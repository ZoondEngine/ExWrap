using Ex.Application.Gilneas.Installer.Core.Installer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex.Application.Gilneas.Installer.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для InstallControl.xaml
    /// </summary>
    public partial class InstallControl : UserControl, IFinalInstallElement
    {
        public InstallControl()
        {
            InitializeComponent();

            var lang = Entry.Language();

            InstallTitle.Content = lang.DefWord( "InstallTitle" );
            GettingManifest.Content = lang.DefWord( "GettingManifest" );
            CalculateFreeSpace.Content = lang.DefWord( "CalculateFreeSpace" );
            DownloadFiles.Content = lang.DefWord( "DownloadFiles" );
            InstallProcess.Content = lang.DefWord( "InstallProcess" );
            InstallCheck.Content = lang.DefWord( "InstallCheck" );
            InstalControlCancel.Content = lang.DefWord( "InstallControlCancel" );
        }

        public bool Install()
        {
            return true;
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            var main = Entry.Window();
            main.ChangeContent( Entry.Installer().Previous().Child() );
        }
    }
}
