using Ex.Application.Gilneas.Installer.Content.Graphics;
using Ex.Application.Gilneas.Installer.Core.Install;

using System;

namespace Ex.Application.Gilneas.Installer
{
    public static class Entry
    {
        private static string m_InstallFolder = "C:/Games/Firelands";
        public static void Execute()
        {
            ExObject.Instantiate<EffectObject>();
            ExObject.Instantiate<InstallerObject>();
        }

        public static void Close()
        {
            Environment.Exit( 0 );
        }

        public static string GetInstallFolder()
            => m_InstallFolder;
        public static void SetInstallFolder( string folder )
            => m_InstallFolder = folder.Replace("\\", "/");

        public static EffectObject Effect()
            => ExObject.FindObjectOfType<EffectObject>();
        public static InstallerObject Installer()
            => ExObject.FindObjectOfType<InstallerObject>();
    }
}
