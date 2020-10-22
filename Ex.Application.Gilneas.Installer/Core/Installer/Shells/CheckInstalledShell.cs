using Ex.Application.Gilneas.Installer.Core.API;
using Ex.Crypto;

namespace Ex.Application.Gilneas.Installer.Core.Installer.Shells
{
    public class CheckInstalledShell : IShell
    {
        public string Description()
        {
            return "checking installed data...";
        }

        public bool Execute()
        {
            var manifest = Entry.Api().Container();
            var hash = ExObject.Instantiate<HashObject>();

            foreach(ManifestElement item in manifest)
            {
                var path = Entry.GetInstallFolder() + item.Local();
                if ( hash.MD5( path ) != item.Hash() )
                    return false;
            }

            return true;
        }
    }
}
