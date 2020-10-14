using Ex.Application.Gilneas.Installer.Content.Graphics;

using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System;
using Ex.Application.Gilneas.Installer.Content.Controls;

namespace Ex.Application.Gilneas.Installer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Entry.Execute();

            InitializeComponent();

            Entry.Installer().Target( this );

            ChangeContent( new SelectionControl() );
        }

        private void Image_MouseEnter( object sender, MouseEventArgs e )
        {
            var image = ( Image ) sender;
            if ( image != null )
            {
                var effect = ExObject.FindObjectOfType<EffectObject>();
                if ( effect != null )
                {
                    effect.SetDropShadow( image, Colors.White, 0.8f );
                }
            }
        }

        private void Image_MouseLeave( object sender, MouseEventArgs e )
        {
            var image = ( Image ) sender;
            if(image != null)
            {
                image.Effect = null;
            }
        }

        private void Image_MouseDown( object sender, MouseButtonEventArgs e )
        {
            Entry.Close();
        }

        public void ChangeContent(UserControl control)
        {
            ContentHolder.Children.Clear();

            ContentHolder.Children.Add( control );
        }

        private void Border_MouseDown( object sender, MouseButtonEventArgs e )
            => MoveWindow( sender, e );

        private void Border_MouseDown_1( object sender, MouseButtonEventArgs e )
            => MoveWindow( sender, e );

        private void Border_MouseDown_2( object sender, MouseButtonEventArgs e )
           => MoveWindow( sender, e );

        private void ContentHolder_MouseDown( object sender, MouseButtonEventArgs e )
            => MoveWindow( sender, e );

        private void MoveWindow( object sender, MouseButtonEventArgs e )
        {
            try
            {
                if ( e.ChangedButton == MouseButton.Left )
                    this.DragMove();
            }
            catch ( Exception ) { }
        }

        private void Minimize_Down( object sender, MouseButtonEventArgs e )
        {
            if ( WindowState != WindowState.Minimized )
            {
                WindowState = WindowState.Minimized;
            }
        }
    }
}
