using System;

namespace Ex.Network
{
    public class BaseNetworkEntity : INetworkConvertiable
    {
        public int Identifier { get; set; }
        public ulong Signature { get; set; } = 0xDEAD_FACE;

        public BaseNetworkEntity( int identifier )
            => Identifier = identifier;

        public virtual bool IsValid()
        {
            return Identifier != 0
                && Signature == 0xDEAD_FACE;
        }

        public virtual T Unbox<T>() where T : BaseNetworkEntity
            => ( T ) this;

        public virtual string ToNetworkString()
        {
            throw new NotImplementedException( $"Method 'ToNetworkString' must be implemented in child entities" );
        }

        public static int GetIdentifierByType<T>() where T : BaseNetworkEntity, new()
            => new T().Identifier;
    }
}
