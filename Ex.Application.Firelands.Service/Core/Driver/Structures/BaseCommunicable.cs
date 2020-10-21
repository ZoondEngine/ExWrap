namespace Ex.Application.Firelands.Service.Core.Driver.Structures
{
    public abstract class BaseCommunicable
    {
        public ulong Signature = 0xDEADFACE;

        public virtual bool Valid()
        {
            return Signature == 0xDEADFACE;
        }
    }
}
