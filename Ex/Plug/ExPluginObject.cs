namespace Ex.Plug
{
    public class ExPluginObject : ExObject
    {
        public ExPluginObject()
            : base()
        { }

        public ExPluginObject(string tag)
            : base(tag)
        { }

        public void Load(string path)
        {
            ExPluginLoader.Read( path );
            ExPluginLoader.Load();
        }
        public void Unload()
        {
            ExPluginLoader.Unload();
            ExPluginLoader.Clear();
        }
        public void Reload(string path)
        {
            Unload();
            Load( path );
        }

        public T Get<T>() where T : IExPlugin
            => ExPluginLoader.Get<T>();
        public void Add<T>() where T : IExPlugin, new()
        {
            ExPluginLoader.Add<T>();
            ExPluginLoader.Load<T>();
        }
        public void Remove<T>() where T : IExPlugin
        {
            ExPluginLoader.Unload<T>();
            ExPluginLoader.Remove<T>();
        }

        public void Subscribe( MessageChangedDelegate mcd )
            => ExPluginLoader.OnMessageChanged += mcd;
        public void Unsibscribe( MessageChangedDelegate mcd )
            => ExPluginLoader.OnMessageChanged -= mcd;

        public int Count()
            => ExPluginLoader.Count();
    }
}
