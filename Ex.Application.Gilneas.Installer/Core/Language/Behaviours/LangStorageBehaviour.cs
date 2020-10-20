using System.Collections.Generic;

namespace Ex.Application.Gilneas.Installer.Core.Language.Behaviours
{
    public class LangStorageBehaviour : ExBehaviour
    {
        private Dictionary<string, Dictionary<string, string>> m_WordsDictionary;
        private LanguageCoreObject m_LanguageCoreObject;

        public LanguageCoreObject Parent()
            => m_LanguageCoreObject;

        public string Word(string locale, string key)
        {
            Dictionary<string, string> dict;
            if (!m_WordsDictionary.ContainsKey(locale))
            {
                dict = m_WordsDictionary[ "en" ];
            }
            else
            {
                dict = m_WordsDictionary[ locale ];
            }

            if ( dict.ContainsKey( key ) )
            {
                return dict[ key ];
            }

            return "unknown";
        }

        public override void Awake()
        {
            m_LanguageCoreObject = Unbox<LanguageCoreObject>( ParentObject );

            m_WordsDictionary = new Dictionary<string, Dictionary<string, string>>()
            {
                [ "ru" ] = new Dictionary<string, string>()
                {
                    [ "OpenFileDialog" ] = "Обзор...",
                    [ "ChoosePath" ] = "Выберите путь установки:",
                    [ "InstallRun" ] = "Установить",
                    [ "Exit" ] = "Выход",
                    [ "SizeDesc" ] = "Общий размер файлов: %size%",
                    [ "InstallTitle" ] = "Установка файлов ...",
                    [ "GettingManifest" ] = "Получение манифеста",
                    [ "CalculateFreeSpace" ] = "Вычисление свободного места",
                    [ "DownloadFiles" ] = "Загрузка файлов",
                    [ "InstallProcess" ] = "Копирование файлов",
                    [ "FinishInstallation" ] = "Завершение установки",
                    [ "InstallControlCancel" ] = "Отмена",

                },

                [ "en" ] = new Dictionary<string, string>()
                {
                    [ "OpenFileDialog" ] = "View...",
                    [ "ChoosePath" ] = "Choose the install path:",
                    [ "InstallRun" ] = "Install",
                    [ "Exit" ] = "Exit",
                    [ "SizeDesc" ] = "Total files size: %size%",
                    [ "InstallTitle" ] = "Installing files ...",
                    [ "GettingManifest" ] = "Getting manifest",
                    [ "CalculateFreeSpace" ] = "Calculate free space",
                    [ "DownloadFiles" ] = "Download files",
                    [ "InstallProcess" ] = "Copy new files",
                    [ "FinishInstallation" ] = "Finishing installation",
                    [ "InstallControlCancel" ] = "Cancel",
                }
            };
        }
    }
}
