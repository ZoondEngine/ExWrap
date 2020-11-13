using System;
using System.Net.Sockets;

namespace Ex.Network.Store
{
    public class TcpSession
    {
        public Guid InternalGuid { get; private set; }
        public TcpClient Native { get; private set; }
        public DateTime ConnectedAt { get; private set; }

        public TcpSession( Guid guid, TcpClient client )
        {
            InternalGuid = guid;
            Native = client;
            ConnectedAt = DateTime.Now;
        }

        public NetworkStream Stream()
            => Native.GetStream();
        public bool IsConnected()
            => Native != null && Native.Connected;

        public void Disconnect()
        {
            if ( IsConnected() )
            {

            }
        }
    }
}
