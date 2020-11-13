using Ex.Network.Exceptions;
using Ex.Network.Packetize;
using Ex.Network.Processors;
using Ex.Network.Security;
using Ex.Network.Store;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex.Network.Server
{
	public class TcpObserver
	{
		private TcpListener m_Listener;

		private readonly TcpObserverSettings m_Settings;
		private List<Task> m_ReceivingTasks;
		private List<TcpSession> m_ActiveSessions;
		private ProcessorStorage m_ProcessorStorage;
		private PacketSerializer m_Serializer;
		private Cryptor m_Cryptor;
		private int m_ConnectedCounter;

		public TcpObserver( TcpObserverSettings settings )
		{
			m_Settings = settings;

			Build();
		}

		private void Build()
		{
			m_ReceivingTasks = new List<Task>( m_Settings.MaxConnections );
			m_ActiveSessions = new List<TcpSession>( m_Settings.MaxConnections );
			m_ProcessorStorage = new ProcessorStorage();
			m_Serializer = new PacketSerializer();
			m_Cryptor = m_Settings.EnableCrypto ? new Cryptor() : null;
		}

		public bool IsListening()
			=> m_Listener != null;

		public void ParseProcessorsFromAssembly( Assembly containingProcessorsAssembly )
			=> m_ProcessorStorage.ParseProcessorsFromAssembly( containingProcessorsAssembly );
		public void AddProcessor<T>() where T : INetworkProcessor, new()
			=> m_ProcessorStorage.AddProcessor<T>();
		public void AddProcessor( INetworkProcessor processor )
			=> m_ProcessorStorage.AddProcessor( processor );
		public void RemoveProcessor( INetworkProcessor processor )
			=> m_ProcessorStorage.RemoveProcessor( processor );

		public void Listen()
		{
			if ( !IsListening() )
			{
				m_ConnectedCounter = 0;

				m_Listener = new TcpListener( new IPEndPoint( m_Settings.IpAddress, m_Settings.Port ) );
				m_Listener.Start();

				//TODO: make good threading
				m_ReceivingTasks.Add( Accepting() );
			}
		}

		public void Shutdown()
		{
			if ( IsListening() )
			{
				m_Listener.Stop();
				Task.WhenAll( m_ReceivingTasks ).Wait();
				m_Listener = null;
			}
		}

		public void Send( BaseNetworkEntity entity, TcpSession session )
		{
			byte[] data = null;

			if ( m_Settings.EnableCrypto )
			{
				var deserialized = m_Serializer.SerializeAsString( entity );
				if ( deserialized != null )
				{
					var crypted = m_Cryptor.Encrypt( deserialized, session.InternalGuid.ToString() );
					data = Encoding.UTF8.GetBytes( $"{session.InternalGuid.ToString()}|{crypted}" );
				}
			}
			else
			{
				data = m_Serializer.Serialize( entity );
			}

			session.Stream().Write( data, 0, data.Length );
		}
		public void Broadcast( BaseNetworkEntity entity )
			=> m_ActiveSessions.ForEach( ( x ) => Send( entity, x ) );

		private async Task Accepting()
		{
			await Task.Run( () =>
			{
				while ( true )
				{
					try
					{
						TcpClient client = m_Listener.AcceptTcpClient();
						if ( m_ConnectedCounter < m_Settings.MaxConnections )
						{
							var session = new TcpSession( Guid.NewGuid(), client );
							m_ActiveSessions.Add( session );
							m_ConnectedCounter++;

							Receiving( session );
						}
					}
					catch ( Exception ex )
					{
						throw new NetworkListeningException( $"{MethodBase.GetCurrentMethod()}: Can't accept client. Error: '{ex.Message}'!" );
					}
				}
			} );
		}

		private void Receiving( TcpSession client )
		{
			new Thread( () => { m_ReceivingTasks.Add( ReceivingTask( client ) ); } ).Start();
		}
		private void Receive( byte[] data, TcpSession session )
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

			var processorResponse = m_ProcessorStorage.Handle( entity );
			if ( processorResponse != null )
			{
				Send( processorResponse, session );
			}
		}
		private async Task ReceivingTask( TcpSession client )
		{
			try
			{
				byte[] buffer = new byte[ 4096 ];
				MemoryStream ms = new MemoryStream();
				while ( client.IsConnected() )
				{
					int bytesRead = await client.Stream().ReadAsync( buffer, 0, buffer.Length );
					ms.Write( buffer, 0, bytesRead );
					if ( !client.Stream().DataAvailable )
					{
						Receive( ms.ToArray(), client );
						ms.Seek( 0, SeekOrigin.Begin );
						ms.SetLength( 0 );
					}
				}
			}
			catch
			{ }

			m_ActiveSessions.Remove( client );
			client.Disconnect();
		}
	}
}
