using Ex.Application.Gilneas.Installer.Core.Settings.Behaviours;
using Ex.Attributes;

namespace Ex.Application.Gilneas.Installer.Core.Settings
{
    [RequiredBehaviour(typeof(SettingsReadBehaviour))]
    public class SettingsCoreObject : ExObject
    {
        public SettingsCoreObject()
            : base()
        { }

        public SettingsCoreObject(string tag)
            : base(tag)
        { }

        public void Load()
            => Reader().Load();

        public void Save()
            => Reader().Save();

        public T Read<T>( string section, string key )
            => Reader().Read<T>( section, key );

        public void Write<T>( string section, string key, T val )
            => Reader().Write( section, key, val );

        public void WriteSave<T>( string section, string key, T val )
            => Reader().WriteSave( section, key, val );

        public SettingsReadBehaviour Reader()
            => GetComponent<SettingsReadBehaviour>();
    }
}
