using Ex.Network.Packetize.Serialize;

namespace Ex.Network.Packetize
{
    internal class PacketSerializer
    {
        private IPacketSerializer m_Serializer;
        public void ChangeSerializer( IPacketSerializer serializer )
            => m_Serializer = serializer;

        public PacketSerializer()
        {
            ChangeSerializer( new Json() );
        }

        public string SerializeAsString( BaseNetworkEntity entity )
            => m_Serializer.SerializeAsString( entity );
        public byte[] Serialize( BaseNetworkEntity entity )
            => m_Serializer.Serialize( entity );
        public T Deserialize<T>( byte[] data ) where T : BaseNetworkEntity
            => m_Serializer.Deserialize<T>( data );

        public string StaticSerializeAsString( BaseNetworkEntity entity )
            => Instance().SerializeAsString( entity );
        public static byte[] StaticSerialize( BaseNetworkEntity entity )
               => Instance().Serialize( entity );
        public static T StaticDeserialize<T>( byte[] data ) where T : BaseNetworkEntity
            => Instance().Deserialize<T>( data );

        private static PacketSerializer m_Instance;
        public static PacketSerializer Instance()
        {
            lock ( m_Instance )
            {
                if ( m_Instance == null )
                {
                    m_Instance = new PacketSerializer();
                }

                return m_Instance;
            }
        }
    }
}
