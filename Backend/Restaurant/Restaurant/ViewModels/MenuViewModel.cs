using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Restaurant.Helps;
using Restaurant.Models;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.Entity;
using Restaurant.Views;

namespace Restaurant.ViewModels
{
    public class MenuViewModel : NotifyPropertyChangedHelp
    {
        private CategoryBLL categoryBLL = new CategoryBLL();
        private MealBLL mealBLL = new MealBLL();
        private List<ProductsDisplay> shoppingList = new List<ProductsDisplay>();

        public MenuViewModel()
        {
            Categories = new ObservableCollection<string>(categoryBLL.GetCategories());
            for (int index = 0; index < Categories.Count; index++)
            {
                Regex regex = new Regex(@"\s*$");
                Categories[index] = regex.Replace(Categories[index], "");
                Categories[index] = Categories[index].ToUpper();
            }
            SelectedItemCombobox = Categories.First();

            ProductsDisplays = new ObservableCollection<ProductsDisplay>(mealBLL.GetProductsByCategory(SelectedItemCombobox));
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
                ProductsDisplays = new ObservableCollection<ProductsDisplay>(mealBLL.GetProductsByCategory(SelectedItemCombobox));
            }
        }

        private ObservableCollection<ProductsDisplay> productsDisplays;
        public ObservableCollection<ProductsDisplay> ProductsDisplays
        {
            get
            {
                return productsDisplays;
            }
            set
            {
                productsDisplays = value;
                NotifyPropertyChanged("ProductsDisplays");
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

        private ICommand addToCartCommand;
        public ICommand AddToCartCommand
        {
            get
            {
                if (addToCartCommand == null)
                {
                    addToCartCommand = new RelayCommand(AddToCart, param => CanExecuteCommand);
                }
                return addToCartCommand;
            }
        }

        public void AddToCart(object param)
        {
            if (SelectedItemList == null)
            {
                MessageBox.Show("Select a product");
            }
            else
            {
                shoppingList.Add(SelectedItemList);
                MessageBox.Show("The product has been added to your cart");
            }
        }

        private ICommand seeCartCommand;
        public ICommand SeeCartCommand
        {
            get
            {
                if (seeCartCommand == null)
                {
                    seeCartCommand = new RelayCommand(SeeCart);
                }
                return seeCartCommand;
            }
        }

        public void SeeCart(object param)
        {
            SeeCartWindow seeCartWindow = new SeeCartWindow();
            SeeCartViewModel seeCartViewModel = new SeeCartViewModel(shoppingList);
            seeCartWindow.DataContext = seeCartViewModel;
            seeCartWindow.ShowDialog();
            shoppingList = new List<ProductsDisplay>();
            foreach (var product in seeCartViewModel.ProductsInCarts)
            {
                shoppingList.Add(new ProductsDisplay()
                {
                    Name = product.Name,
                    Quantity = product.Pieces,
                    Price = product.Price
                });
            }
        }
        #endregion
    }
}
