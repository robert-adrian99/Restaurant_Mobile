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
    /// Interaction logic for ProductNearExhaustionWindow.xaml
    /// </summary>
    public partial class ProductNearExhaustionWindow : Window
    {
        public ProductNearExhaustionWindow()
        {
            InitializeComponent();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            EmployeeStartWindow employeeStartWindow = new EmployeeStartWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = employeeStartWindow;
            App.Current.MainWindow.Show();
        }
    }
}
