﻿using System;
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
    public partial class InstallControl : UserControl
    {
        public InstallControl()
        {
            InitializeComponent();
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            var main = ( MainWindow ) Entry.Installer().Target();
            main.ChangeContent( new SelectionControl() );
        }
    }
}
