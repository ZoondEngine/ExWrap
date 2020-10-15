using Ex.Application.Gilneas.Installer.Content.Graphics;
using Ex.Application.Gilneas.Installer.Core.API;
using Ex.Application.Gilneas.Installer.Core.Installer;
using Ex.Application.Gilneas.Installer.Core.Language;

using System;

namespace Ex.Application.Gilneas.Installer
{
    public static class Entry
    {
        private static string m_InstallFolder = "C:/Games/Firelands";
        private static IChangable m_ChangableWindow = null;

        public static void Execute( IChangable changableWindow )
        {
            ExObject.Instantiate<EffectObject>();
            ExObject.Instantiate<ApiCoreObject>();
            ExObject.Instantiate<LanguageCoreObject>();
            ExObject.Instantiate<InstallCoreObject>();

            m_ChangableWindow = changableWindow;

            //Api().LoadInclude();
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
        public static InstallCoreObject Installer()
            => ExObject.FindObjectOfType<InstallCoreObject>();
        public static ApiCoreObject Api()
            => ExObject.FindObjectOfType<ApiCoreObject>();
        public static LanguageCoreObject Language()
            => ExObject.FindObjectOfType<LanguageCoreObject>();
        public static IChangable Window()
            => m_ChangableWindow;
    }
}
