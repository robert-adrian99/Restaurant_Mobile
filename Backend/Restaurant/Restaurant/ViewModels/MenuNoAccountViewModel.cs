using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Restaurant.Helps;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.Entity;
using Restaurant.Views;

namespace Restaurant.ViewModels
{
    public class MenuNoAccountViewModel : NotifyPropertyChangedHelp
    {
        private CategoryBLL categoryBLL = new CategoryBLL();
        private MealBLL mealBLL = new MealBLL();

        public MenuNoAccountViewModel()
        {
            Categories = new ObservableCollection<string>(categoryBLL.GetCategories());
            for (int index = 0; index < Categories.Count; index++)
            {
                Regex regex = new Regex(@"\s*$");
                Categories[index] = regex.Replace(Categories[index], "");
                Categories[index] = Categories[index].ToUpper();
            }
            SelectedItemCombobox = Categories.First();

            ProductsDisplay = new ObservableCollection<ProductsDisplay>(mealBLL.GetProductsByCategory(SelectedItemCombobox));
        }

        #region DataMembers
        public ObservableCollection<string> Categories { get; }

        private string selectedItemCombobox;
        public string SelectedItemCombobox
        {
            get
            {
                return selectedItemCombobox;
            }
            set
            {
                selectedItemCombobox = value;
                CanExecuteCommand = false;
                NotifyPropertyChanged("SelectedItemCombobox");
                ProductsDisplay = new ObservableCollection<ProductsDisplay>(mealBLL.GetProductsByCategory(SelectedItemCombobox));
            }
        }

        private ObservableCollection<ProductsDisplay> productsDisplay;
        public ObservableCollection<ProductsDisplay> ProductsDisplay
        {
            get
            {
                return productsDisplay;
            }
            set
            {
                productsDisplay = value;
                NotifyPropertyChanged("ProductsDisplay");
            }
        }

        private ProductsDisplay selectedItemList;
        public ProductsDisplay SelectedItemList
        {
            get
            {
                return selectedItemList;
            }
            set
            {
                selectedItemList = value;
                CanExecuteCommand = true;
                NotifyPropertyChanged("SelectedItemList");
            }
        }
        #endregion

        #region CommandMembers
        private bool CanExecuteCommand { get; set; } = false;
        private ICommand seeDetailsCommand;
        public ICommand SeeDetailsCommand
        {
            get
            {
                if (seeDetailsCommand == null)
                {
                    seeDetailsCommand = new RelayCommand(SeeDetails, param => CanExecuteCommand);
                }
                return seeDetailsCommand;
            }
        }

        public void SeeDetails(object param)
        {
            if (SelectedItemList == null)
            {
                MessageBox.Show("Select a product");
            }
            else
            {
                DetailsWindow detailsWindow = new DetailsWindow();
                DetailsViewModel detailsViewModel = new DetailsViewModel(selectedItemList.Name);
                detailsWindow.DataContext = detailsViewModel;
                detailsWindow.ShowDialog();
            }
        }
        #endregion
    }
}
