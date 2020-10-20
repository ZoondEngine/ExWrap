using Ex.Application.Gilneas.Installer.Core.API;
using Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities;
using Ex.Application.Gilneas.Installer.Core.Installer;
using Ex.Exceptions;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MessageBox = System.Windows.Forms.MessageBox;
using Uri = System.Uri;
using UserControl = System.Windows.Controls.UserControl;
using MethodInvoker = System.Windows.Forms.MethodInvoker;
using System.Windows.Media;

namespace Ex.Application.Gilneas.Installer.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для InstallControl.xaml
    /// </summary>
    public partial class InstallControl : UserControl, IFinalInstallElement
    {
        private delegate void StartInstall( int count );
        private delegate void FinishInstall( bool success );
        private delegate void ProcessElement( ManifestElement element );

        private event StartInstall OnStart;
        private event ProcessElement OnProcess;
        private event FinishInstall OnFinish;

        public InstallControl()
        {
            InitializeComponent();

            var lang = Entry.Language();

            InstallTitle.Content = lang.DefWord( "InstallTitle" );
            GettingManifest.Content = lang.DefWord( "GettingManifest" );
            CalculateFreeSpace.Content = lang.DefWord( "CalculateFreeSpace" );
            InstallProcess.Content = lang.DefWord( "InstallProcess" );
            FinishInstallation.Content = lang.DefWord( "FinishInstallation" );
            InstalControlCancel.Content = lang.DefWord( "InstallControlCancel" );

            OnStart += InstallControl_OnStart;
            OnProcess += InstallControl_OnProcess;
            OnFinish += InstallControl_OnFinish;
        }

        private void InstallControl_OnFinish( bool success )
        {
            if(!success)
            {
                Entry.Close();
            }
            else
            {
                Dispatcher.BeginInvoke( ( MethodInvoker ) ( () =>
                {
                    Entry.Window().ChangeContent( new FinishControl() );
                } ) );
            }
        }

        private void InstallControl_OnProcess( ManifestElement element )
        {
            Dispatcher.BeginInvoke( ( MethodInvoker ) ( () =>
            {
                InstallProgress.Value++;
            }));
        }

        private void InstallControl_OnStart( int count )
        {
            Dispatcher.BeginInvoke( ( MethodInvoker ) ( () =>
            {
                InstallProgress.Maximum = count;
                InstallProgress.Minimum = 0;
                InstallProgress.Value = 0;
            }));
        }

        public bool Install()
        {
            ExecuteAsync();
            return true;
        }

        private async void ExecuteAsync()
        {
            await Task.Run( () =>
            {
                Execute();
            });
        }

        private void Execute()
        {
            OnStart(Entry.Api().Files());

            Entry.Api().Manifest();
            var container = Entry.Api().Container();

            UnfollowStep( GettingManifest );

            if ( !IsRequiredSpaceFree( Entry.GetInstallFolder(), Entry.Api().Size() ) )
            {
                MessageBox.Show( "Mb_ErrorNEFreeSpace_Description", "Mb_ErrorNEFreeSpace_Title", System.Windows.Forms.MessageBoxButtons.OK );
                OnFinish( false );
            }

            UnfollowStep( CalculateFreeSpace );

            WebClient client = new WebClient();

            foreach (ManifestElement item in container)
            {
                try
                {
                    var localRepaired = item.Local().Replace( "\"", "" ).Replace( "\\", "/" );
                    var fullpath = Entry.GetInstallFolder() + localRepaired;

                    if ( !Directory.Exists( Path.GetDirectoryName( fullpath ) ) )
                        Directory.CreateDirectory( Path.GetDirectoryName( fullpath ) );

                    client.DownloadFile( new Uri( item.Remote().Replace( "\"", "" ) ), fullpath );
                }
                catch
                {
                    MessageBox.Show( "Mb_ErrorDownloadError_Description", "Mb_ErrorDownloadError_Title", System.Windows.Forms.MessageBoxButtons.OK );

                    OnFinish( false );
                    break;
                }

                OnProcess( item );
            }

            UnfollowStep( InstallProcess );

            if(!Entry.Installer().Shell().Execute())
            {
                MessageBox.Show( "Mb_ErrorShells_Description", "Mb_ErrorShells_Title", System.Windows.Forms.MessageBoxButtons.OK );
                OnFinish( false );
            }

            OnFinish( true );
        }

        private bool IsRequiredSpaceFree(string path, long needsSpace)
        {
            var driveInfo = new DriveInfo( path );
            return driveInfo.AvailableFreeSpace > needsSpace;
        }

        private void Clean()
        {

        }

        private void UnfollowStep(Label label)
        {
            Dispatcher.BeginInvoke( ( MethodInvoker ) ( () =>
            {
                label.Foreground = new SolidColorBrush( Color.FromArgb( 255, 56, 56, 56 ) );
            } ) );
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            Clean();
            Entry.Window().ChangeContent( Entry.Installer().Previous().Child() );
        }
    }
}
