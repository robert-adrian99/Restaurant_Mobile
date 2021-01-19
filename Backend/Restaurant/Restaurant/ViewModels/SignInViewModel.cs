using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Restaurant.Helps;
using Restaurant.Models;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Views;

namespace Restaurant.ViewModels
{
    public class SignInViewModel : NotifyPropertyChangedHelp
    {
        #region DataMembers
        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                ErrorMessage = "";
                Regex regex = new Regex(@"^[A-Za-z0-9._]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
                if (regex.Match(Email) == Match.Empty)
                {
                    ErrorMessage = "INVALID EMAIL FORMAT";
                    CanExecuteCommand = false;
                }
                else
                {
                    CanExecuteCommand = true;
                }
                NotifyPropertyChanged("Email");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }
        #endregion

        #region CommandMembers
        public bool CanExecuteCommand { get; set; } = false;

        private ICommand signInCommand;
        public ICommand SignInCommand
        {
            get
            {
                if (signInCommand == null)
                {
                    signInCommand = new RelayCommand(SignIn, param => CanExecuteCommand);
                }
                return signInCommand;
            }
        }

        public void SignIn(object param)
        {
            string password = (param as PasswordBox).Password;
            if (password.Length == 0)
            {
                MessageBox.Show("Enter your password");
            }
            else
            {
                UserBLL user = new UserBLL();
                try
                {
                    user.SignIn(Email, password);
                    Regex regex = new Regex(@"@steak-house.com$|@steakhouse.com$");
                    if (regex.Match(Email) != Match.Empty)
                    {
                        EmployeeStartWindow employeeStartWindow = new EmployeeStartWindow();
                        App.Current.MainWindow.Close();
                        App.Current.MainWindow = employeeStartWindow;
                        App.Current.MainWindow.Show();
                    }
                    else
                    {
                        MenuWindow menuWindow = new MenuWindow();
                        App.Current.MainWindow.Close();
                        App.Current.MainWindow = menuWindow;
                        App.Current.MainWindow.Show();
                    }
                }
                catch
                {
                    MessageBox.Show("Incorrect email or password!");
                }
            }
        }
        #endregion
    }
}
