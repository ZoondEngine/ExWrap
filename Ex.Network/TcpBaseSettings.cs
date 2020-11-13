using System.Net;

namespace Ex.Network
{
    public class TcpBaseSettings
    {
        public IPAddress IpAddress { get; private set; }
        public int Port { get; private set; }
        public bool EnableCrypto { get; private set; }

        public TcpBaseSettings SetIpAddress( string ipAddress )
            => SetIpAddress( IPAddress.Parse( ipAddress ) );
        public TcpBaseSettings SetIpAddress( IPAddress ipAddress )
        {
            IpAddress = ipAddress;
            return this;
        }

        public TcpBaseSettings SetPort( int port )
        {
            Port = port;
            return this;
        }

        public TcpBaseSettings Crypto( bool enable )
        {
            EnableCrypto = enable;
            return this;
        }
    }
}
