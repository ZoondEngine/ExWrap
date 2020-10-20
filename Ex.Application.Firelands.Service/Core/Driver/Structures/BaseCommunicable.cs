namespace Ex.Application.Firelands.Service.Core.Driver.Structures
{
    public abstract class BaseCommunicable
    {
        public ulong Signature = 0xDEADFACE;

        public abstract bool Valid();
    }
}
