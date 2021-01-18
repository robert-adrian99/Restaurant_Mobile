using Restaurant.Models;
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
using System.Windows.Shapes;

namespace Restaurant.Views
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void SignInClick(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = signInWindow;
            App.Current.MainWindow.Show();
        }

        private void SignUpClick(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = signUpWindow;
            App.Current.MainWindow.Show();
        }

        private void NoAccountClick(object sender, RoutedEventArgs e)
        {
            MenuNoAccountWindow menuNoAccountWindow = new MenuNoAccountWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = menuNoAccountWindow;
            App.Current.MainWindow.Show();
        }

    }
}
