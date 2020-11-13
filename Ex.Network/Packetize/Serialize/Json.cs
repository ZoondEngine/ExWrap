using Ex.Network.Exceptions;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace Ex.Network.Packetize.Serialize
{
    public class Json : IPacketSerializer
    {
        public T Deserialize<T>( byte[] data ) where T : BaseNetworkEntity
        {
            var str = Encoding.UTF8.GetString( data );
            if ( str != null )
            {
                var deserialized = ( T ) JsonConvert.DeserializeObject( str, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All } );
                if ( deserialized != null )
                {
                    return deserialized;
                }
                else
                {
#if DEBUG
                    throw new NetworkSerializeException( $"{MethodBase.GetCurrentMethod()}: Serialize error! Can't rebuild json string to requred object '{typeof( T ).FullName}'!" );
#endif
                }
            }
            else
            {
#if DEBUG
                throw new NetworkSerializeException( $"{MethodBase.GetCurrentMethod()}: Serialize error! Can't get string from byte data!" );
#endif
            }
#if DEBUG
            throw new NetworkSerializeException( $"{MethodBase.GetCurrentMethod()}: Serialize wrong and return null!" );
#else
            return null;
#endif
        }

        public byte[] Serialize( BaseNetworkEntity entity )
            => Encoding.UTF8.GetBytes( $"{entity.Identifier}|{SerializeObjectInternal( entity )}" );

        public string SerializeAsString( BaseNetworkEntity entity )
        {
            return $"{entity.Identifier}|{SerializeObjectInternal( entity )}";
        }

        private string SerializeObjectInternal( BaseNetworkEntity entity )
        {
            return JsonConvert.SerializeObject(
                entity.ToNetworkString(),
                Formatting.Indented,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All } );
        }
    }
}
