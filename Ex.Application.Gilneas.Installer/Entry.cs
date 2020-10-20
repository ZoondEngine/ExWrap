using Ex.Application.Gilneas.Installer.Content.Graphics;
using Ex.Application.Gilneas.Installer.Core.API;
using Ex.Application.Gilneas.Installer.Core.Installer;
using Ex.Application.Gilneas.Installer.Core.Language;
using Ex.Application.Gilneas.Installer.Core.Settings;

using System;

namespace Ex.Application.Gilneas.Installer
{
    public static class Entry
    {
        private static string m_InstallFolder = "C:/Games/Firelands";
        private static string m_ExecutableFile = "\\Battle.net Launcher.exe";
        private static IChangable m_ChangableWindow = null;

        public static void Execute( IChangable changableWindow )
        {
            ExObject.Instantiate<SettingsCoreObject>();
            Settings().Load();

            ExObject.Instantiate<EffectObject>();
            ExObject.Instantiate<ApiCoreObject>();
            Api().Include();

            ExObject.Instantiate<LanguageCoreObject>();
            ExObject.Instantiate<InstallCoreObject>();

            m_ChangableWindow = changableWindow;
        }

        public static void Close()
        {
            Settings().Save();

            Environment.Exit( 0 );
        }

        public static string GetInstallFolder()
            => m_InstallFolder;
        public static void SetInstallFolder( string folder )
            => m_InstallFolder = folder.Replace("\\", "/");

        public static string GetExecutable()
            => m_ExecutableFile;

        public static SettingsCoreObject Settings()
            => ExObject.FindObjectOfType<SettingsCoreObject>();
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
