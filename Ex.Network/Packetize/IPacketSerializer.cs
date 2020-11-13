namespace Ex.Network.Packetize
{
    public interface IPacketSerializer
    {
        string SerializeAsString( BaseNetworkEntity entity );
        byte[] Serialize( BaseNetworkEntity entity );
        T Deserialize<T>( byte[] data ) where T : BaseNetworkEntity;
    }
}
