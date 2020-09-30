namespace Ex.Plug
{
    public interface IExPlugin
    {
        string GetName();
        string GetVersion();
        string GetIdentifier();

        T As<T>() where T : IExPlugin;

        void OnLoad();
        void OnUnload();
    }
}
