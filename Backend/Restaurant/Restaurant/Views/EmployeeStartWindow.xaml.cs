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
    /// Interaction logic for EmployeeStartWindow.xaml
    /// </summary>
    public partial class EmployeeStartWindow : Window
    {
        public EmployeeStartWindow()
        {
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            StartWindow startWindow = new StartWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = startWindow;
            App.Current.MainWindow.Show();
        }

        private void SeeOrdersClick(object sender, RoutedEventArgs e)
        {
            EmployeeViewOrdersWindow employeeViewOrdersWindow = new EmployeeViewOrdersWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = employeeViewOrdersWindow;
            App.Current.MainWindow.Show();
        }            
        
        private void SeeProductsClick(object sender, RoutedEventArgs e)
        {
            ProductNearExhaustionWindow productNearExhaustionWindow = new ProductNearExhaustionWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = productNearExhaustionWindow;
            App.Current.MainWindow.Show();
        }
    }
}
