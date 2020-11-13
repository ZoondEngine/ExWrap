namespace Ex.Network
{
    public interface INetworkProcessor
    {
        bool IsHandlablePacket( BaseNetworkEntity entity );
        BaseNetworkEntity Handle( BaseNetworkEntity entity );
    }
}
