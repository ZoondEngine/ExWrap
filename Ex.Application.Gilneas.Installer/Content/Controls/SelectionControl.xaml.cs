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
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            using ( var dialog = new WinForms.FolderBrowserDialog() )
            {
                dialog.Description = "Путь установки: ";
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
            var main = ( MainWindow ) Entry.Installer().Target();
            main.ChangeContent( new InstallControl() );
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            Entry.Close();
        }
    }
}
