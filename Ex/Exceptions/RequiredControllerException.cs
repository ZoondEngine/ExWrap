namespace Ex.Exceptions
{
    public class RequiredControllerException : ExException
    {
        public RequiredControllerException(string message)
            : base(message)
        { }
    }
}
