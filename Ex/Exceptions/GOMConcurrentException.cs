namespace Ex.Exceptions
{
    public class GOMConcurrentException : ExException
    {
        public GOMConcurrentException(string message)
            : base(message)
        { }
    }
}
