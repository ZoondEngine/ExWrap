namespace Ex.Network.Server
{
    public class TcpObserverSettings : TcpBaseSettings
    {
        public int MaxConnections { get; private set; }

        public TcpObserverSettings SetMaxConnections( int maxConnections )
        {
            MaxConnections = maxConnections;
            return this;
        }
    }
}
