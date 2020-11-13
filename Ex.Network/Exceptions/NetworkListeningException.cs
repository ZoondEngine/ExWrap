namespace Ex.Network.Exceptions
{
    public class NetworkListeningException : BaseNetworkException
    {
        public NetworkListeningException( string message )
            : base( message )
        { }
    }
}
