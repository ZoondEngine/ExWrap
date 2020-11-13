namespace Ex.Network.Exceptions
{
    public class NetworkConnectionException : BaseNetworkException
    {
        public NetworkConnectionException( string message )
            : base( message )
        { }
    }
}
