using Ex.Application.Gilneas.Installer.Core.Language;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using WinForms = System.Windows.Forms;

namespace Ex.Application.Gilneas.Installer.Content.Controls
{
    /// <summary>
    /// Логика взаимодействия для SelectionControl.xaml
    /// </summary>
    public partial class SelectionControl : UserControl
    {
        public SelectionControl()
        {
            InitializeComponent();

            InstallFolder.Text = Entry.GetInstallFolder();

            var lang = Entry.Language();

            OpenFileDialog.Content = lang.DefWord( "OpenFileDialog" );
            InstallRun.Content = lang.DefWord( "InstallRun" );
            ChoosePath.Content = lang.DefWord( "ChoosePath" );
            Exit.Content = lang.DefWord( "Exit" );
            SizeDesc.Content = LanguageCoreObject.Replace( lang.DefWord( "SizeDesc" ), "%size%", Entry.Api().Humanized(Entry.Api().Size()) );
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            using ( var dialog = new WinForms.FolderBrowserDialog() )
            {
                dialog.Description = Entry.Language().DefWord("ChoosePath");
                dialog.SelectedPath = Entry.GetInstallFolder();
                dialog.ShowNewFolderButton = true;

                var result = dialog.ShowDialog();
                if ( result == WinForms.DialogResult.OK )
                {
                    Entry.SetInstallFolder( dialog.SelectedPath );
                    InstallFolder.Text = Entry.GetInstallFolder();
                }
            }
        }

        private void Button_Click_2( object sender, RoutedEventArgs e )
        {
            var main = Entry.Window();
            main.ChangeContent( Entry.Installer().Next().Child() );
            Entry.Installer().Install();
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            Entry.Close();
        }
    }
}
