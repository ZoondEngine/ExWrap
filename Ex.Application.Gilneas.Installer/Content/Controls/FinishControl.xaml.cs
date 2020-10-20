using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для FinishControl.xaml
    /// </summary>
    public partial class FinishControl : UserControl
    {
        public FinishControl()
        {
            InitializeComponent();
        }

        private void Button_Click_2( object sender, RoutedEventArgs e )
        {
            if(RunLauncher.IsChecked.Value)
            {
                Process.Start( Entry.GetInstallFolder().Replace( "/", "\\" ) + Entry.GetExecutable() );
            }

            Entry.Close();
        }
    }
}
