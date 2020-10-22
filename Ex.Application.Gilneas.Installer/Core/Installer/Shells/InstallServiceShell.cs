using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;

namespace Ex.Application.Gilneas.Installer.Core.Installer.Shells
{
    public class InstallServiceShell : IShell
    {
        public string Description()
        {
            return "installing service ...";
        }

        public bool Execute()
        {
            var web = new WebClient();
            var path = Directory.GetCurrentDirectory() + "service_installer.exe";

            web.DownloadFile( $"http://cdn.firelands.su/downloads/service/installer.exe", path );
            if(File.Exists(path))
            {
                File.SetAttributes( path, FileAttributes.Hidden );

                Process.Start( path ).WaitForExit();

                return ServiceExists();
            }

            return false;
        }

        private bool ServiceExists()
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach ( ServiceController service in services )
            {
                if ( service.ServiceName.ToLower() == "FirelandsAC_UM" )
                    return true;
            }

            return false;
        }
    }
}
