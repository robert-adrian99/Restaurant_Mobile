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
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Views;

namespace Restaurant.ViewModels
{
    public class SignUpViewModel : NotifyPropertyChangedHelp
    {
        #region DataMembers
        private bool ValidFirstName { get; set; } = false;
        private string firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                Regex regex = new Regex(@"^[A-Z]{1}[a-z]+");
                if (regex.Match(firstName) == Match.Empty)
                {
                    ErrorMessage = "INVALID FIRST NAME";
                    ValidFirstName = false;
                    CanExecuteCommand = false;
                }
                else
                {
                    if (ErrorMessage == "INVALID FIRST NAME")
                    {
                        ErrorMessage = "";
                    }
                    ValidFirstName = true;
                    if (ValidFirstName && ValidLastName && ValidEmail && ValidPhoneNumber && ValidAddress)
                    {
                        CanExecuteCommand = true;
                    }
                }
                NotifyPropertyChanged("FirstName");
            }
        }

        private bool ValidLastName { get; set; } = false;
        private string lastName;
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                Regex regex = new Regex(@"^[A-Z]{1}[a-z]+");
                if (regex.Match(lastName) == Match.Empty)
                {
                    ErrorMessage = "INVALID LAST NAME";
                    ValidLastName = false;
                    CanExecuteCommand = false;
                }
                else
                {
                    if (ErrorMessage == "INVALID LAST NAME")
                    {
                        ErrorMessage = "";
                    }
                    ValidLastName = true;
                    if (ValidFirstName && ValidLastName && ValidEmail && ValidPhoneNumber && ValidAddress)
                    {
                        CanExecuteCommand = true;
                    }
                }
                NotifyPropertyChanged("LastName");
            }
        }

        private bool ValidEmail { get; set; } = false;
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
                Regex regex = new Regex(@"^[A-Za-z0-9._]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
                if (regex.Match(email) == Match.Empty)
                {
                    ErrorMessage = "INVALID EMAIL FORMAT";
                    ValidEmail = false;
                    CanExecuteCommand = false;
                }
                else
                {
                    if (ErrorMessage == "INVALID EMAIL FORMAT")
                    {
                        ErrorMessage = "";
                    }
                    ValidEmail = true;
                    if (ValidFirstName && ValidLastName && ValidEmail && ValidPhoneNumber && ValidAddress)
                    {
                        CanExecuteCommand = true;
                    }
                }
                NotifyPropertyChanged("Email");
            }
        }

        private bool ValidPhoneNumber { get; set; } = false;
        private string phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                phoneNumber = value;
                Regex regex = new Regex(@"^0[0-9]{9}");
                if (regex.Match(phoneNumber) == Match.Empty)
                {
                    ErrorMessage = "INVALID PHONE NUMBER FORMAT";
                    ValidPhoneNumber = false;
                    CanExecuteCommand = false;
                }
                else
                {
                    if (ErrorMessage == "INVALID PHONE NUMBER FORMAT")
                    {
                        ErrorMessage = "";
                    }
                    ValidPhoneNumber = true;
                    if (ValidFirstName && ValidLastName && ValidEmail && ValidPhoneNumber && ValidAddress)
                    {
                        CanExecuteCommand = true;
                    }
                }
                NotifyPropertyChanged("PhoneNumber");
            }
        }

        private bool ValidAddress { get; set; } = false;
        private string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                Regex regex = new Regex(@"[A-Za-z\s]+[0-9]+");
                if (regex.Match(address) == Match.Empty)
                {
                    ErrorMessage = "PLEASE ENTER A DELIVERY ADDRESS";
                    ValidAddress = false;
                    CanExecuteCommand = false;
                }
                else
                {
                    if (ErrorMessage == "PLEASE ENTER A DELIVERY ADDRESS")
                    {
                        ErrorMessage = "";
                    }
                    ValidAddress = true;
                    if (ValidFirstName && ValidLastName && ValidEmail && ValidPhoneNumber && ValidAddress)
                    {
                        CanExecuteCommand = true;
                    }
                }
                NotifyPropertyChanged("Address");
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
        private bool CanExecuteCommand { get; set; } = false;
        private ICommand signUpCommand;
        public ICommand SignUpCommand
        {
            get
            {
                if (signUpCommand == null)
                {
                    signUpCommand = new RelayCommand(SignUp, param => CanExecuteCommand);
                }
                return signUpCommand;
            }
        }

        public void SignUp(object param)
        {
            string password = (param as PasswordBox).Password;

            Regex regex = new Regex(@"@steak-house.com$|@steakhouse.com$");
            if (regex.Match(email) != Match.Empty)
            {
                MessageBox.Show("Only the admin can create\nan employee account!", "Account creation failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UserBLL user = new UserBLL();
                if (!user.SignUp(FirstName, LastName, Email, PhoneNumber, Address, password))
                {
                    MessageBox.Show("There is already an account with this email");
                }
                else
                {
                    MenuWindow menuWindow = new MenuWindow();
                    App.Current.MainWindow.Close();
                    App.Current.MainWindow = menuWindow;
                    App.Current.MainWindow.Show();
                }
            }
        }
        #endregion
    }
}
