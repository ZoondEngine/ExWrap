namespace Ex.Network.Client
{
    public class TcpSubscriberSettings : TcpBaseSettings
    {
        public int Timeout { get; private set; }

        public TcpSubscriberSettings SetTimeout( int seconds )
        {
            Timeout = seconds;
            return this;
        }
    }
}
