using Ex.Network.Exceptions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex.Network.Client
{
    public class TcpSubscriber
    {
        private delegate void DataReceivingDelegate( BaseNetworkEntity entity );
        private event DataReceivingDelegate OnDataReceived;

        private readonly TcpSubscriberSettings m_Settings;
        private List<Task> m_ActiveTasks;
        private TcpSession m_Session;
        private PacketSerializer m_Serializer;
        private Cryptor m_Cryptor;

        public TcpSubscriber( TcpSubscriberSettings settings )
        {
            m_Settings = settings;

            Build();
        }

        private void Build()
        {
            m_ActiveTasks = new List<Task>();
            m_Serializer = new PacketSerializer();
            m_Cryptor = m_Settings.EnableCrypto ? new Cryptor() : null;
        }

        public bool IsConnected()
            => m_Session != null && m_Session.IsConnected();

        public bool Connect()
        {
            try
            {
                if ( !IsConnected() )
                {
                    var client = new TcpClient();
                    client.Connect( new IPEndPoint( m_Settings.IpAddress, m_Settings.Port ) );
                    m_Session = new TcpSession( Guid.NewGuid(), client );

                    Receiving();
                    return true;
                }
                else
                {
                    return true;
                }
            }
#if DEBUG
            catch ( Exception ex )
            {
                throw new NetworkConnectionException( $"{MethodBase.GetCurrentMethod()}: Error while trying connect '{ex.Message}'" );
            }
#else
            catch
            {
                return false;
            }
#endif
        }
        public bool Disconnect()
        {
            try
            {
                if ( IsConnected() )
                {
                    m_Session.Disconnect();
                    m_Session = null;

                    return true;
                }
                else
                {
                    return true;
                }
            }
#if DEBUG
            catch ( Exception ex )
            {
                throw new NetworkConnectionException( $"{MethodBase.GetCurrentMethod()}: Error while trying disconnect '{ex.Message}'" );
            }
#else
            catch
            {
                return false;
            }
#endif
        }

        public void Send( BaseNetworkEntity entity )
        {
            byte[] data = null;

            if ( m_Settings.EnableCrypto )
            {
                var deserialized = m_Serializer.SerializeAsString( entity );
                if ( deserialized != null )
                {
                    var crypted = m_Cryptor.Encrypt( deserialized, m_Session.InternalGuid.ToString() );
                    data = Encoding.UTF8.GetBytes( $"{m_Session.InternalGuid.ToString()}|{crypted}" );
                }
            }
            else
            {
                data = m_Serializer.Serialize( entity );
            }

            m_Session.Stream().Write( data, 0, data.Length );
        }
        public T SendAsync<T>( BaseNetworkEntity entity, int awaitableId ) where T : BaseNetworkEntity
        {
            T reply = null;
            DataReceivingDelegate receiveAction = new DataReceivingDelegate( ( response ) =>
            {
                if ( response.Identifier == awaitableId )
                    reply = response.Unbox<T>();
            } );

            OnDataReceived += receiveAction;
            Send( entity );

            var sw = new Stopwatch();
            sw.Start();

            //while(reply == null && sw.Elapsed < TimeSpan.FromSeconds(m_Settings.Timeout))
            while ( reply == null && sw.Elapsed < TimeSpan.FromSeconds( 15 ) )
            {
                Thread.Sleep( 100 );
            }

            sw.Stop();

            OnDataReceived -= receiveAction;
            return reply;
        }

        private void Receiving()
        {
            new Thread( () => m_ActiveTasks.Add( ReceivingTask() ) );
        }
        private void Receive( byte[] data )
        {
            BaseNetworkEntity entity = null;
            if ( m_Settings.EnableCrypto )
            {
                var dataString = Encoding.UTF8.GetString( data );
                var splitted = dataString.Split( '|' );
                if ( splitted.Length == 2 )
                {
                    var decrypted = m_Cryptor.Decrypt( splitted[ 1 ], splitted[ 0 ] );
                    if ( decrypted != null )
                    {
                        entity = m_Serializer.Deserialize<BaseNetworkEntity>( Encoding.UTF8.GetBytes( decrypted ) );
                    }
                }
            }
            else
            {
                entity = m_Serializer.Deserialize<BaseNetworkEntity>( data );
            }

            OnDataReceived?.Invoke( entity );
        }
        private async Task ReceivingTask()
        {
            try
            {
                byte[] buffer = new byte[ 4096 ];
                MemoryStream ms = new MemoryStream();
                while ( m_Session.IsConnected() )
                {
                    int bytesRead = await m_Session.Stream().ReadAsync( buffer, 0, buffer.Length );
                    ms.Write( buffer, 0, bytesRead );
                    if ( !m_Session.Stream().DataAvailable )
                    {
                        Receive( ms.ToArray() );
                        ms.Seek( 0, SeekOrigin.Begin );
                        ms.SetLength( 0 );
                    }
                }
            }
            catch
            { }

            Disconnect();
        }
    }
}
