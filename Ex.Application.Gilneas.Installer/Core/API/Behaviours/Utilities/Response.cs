namespace Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities
{
    public class Response<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }

        public Response()
        { }
    }
}
