using Microsoft.Win32;

namespace Ex.Application.Gilneas.Installer.Core.Install.Scripts
{
    public class SetRegistryInstallScript : IInstallScript
    {
        public bool Execute()
        {
            try
            {
                var firelandsBranch = Registry.LocalMachine.CreateSubKey( "Firelands" );
                using ( var settings = firelandsBranch.CreateSubKey( "Settings" ) )
                {
                    settings.SetValue( "LANG", "ru" );
                    settings.SetValue( "BETA", 0 );
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Name()
            => "fixing-registry";
    }
}
