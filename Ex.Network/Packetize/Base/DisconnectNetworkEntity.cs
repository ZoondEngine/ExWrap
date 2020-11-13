namespace Ex.Network.Packetize
{
    internal class DisconnectNetworkEntity : BaseNetworkEntity
    {
        public string Reason { get; set; }

        public DisconnectNetworkEntity()
            : base( 0xDEAD )
        { }
    }
}
