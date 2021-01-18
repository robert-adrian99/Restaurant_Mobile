using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Restaurant.Helps;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.Entity;
using Restaurant.Views;

namespace Restaurant.ViewModels
{
    public class SearchViewModel : NotifyPropertyChangedHelp
    {
        #region DataMembers
        public string SelectedProductAllergen { get; set; }
        public string SelectedContaining { get; set; }
        private string searchBar;
        public string SearchBar
        {
            get
            {
                return searchBar;
            }
            set
            {
                searchBar = value;
                NotifyPropertyChanged("SearchBar");
            }
        }

        private ObservableCollection<ProductsDisplay> productsDisplay = new ObservableCollection<ProductsDisplay>();
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
        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new RelayCommand(Search);
                }
                return searchCommand;
            }
        }

        public void Search(object param)
        {
            MealBLL meal = new MealBLL();
            if (SearchBar == "")
            {
                MessageBox.Show("Please insert some text to search for");
            }
            else
            {
                if (SelectedProductAllergen.Contains("PRODUCT"))
                {
                    if (SelectedContaining.Contains("DOESN'T"))
                    {
                        ProductsDisplay = new ObservableCollection<ProductsDisplay>(meal.GetProductsContainingInName(SearchBar, false));
                    }
                    else
                    {
                        ProductsDisplay = new ObservableCollection<ProductsDisplay>(meal.GetProductsContainingInName(SearchBar, true));
                    }
                }
                else
                {
                    if (SelectedContaining.Contains("DOESN'T"))
                    {
                        ProductsDisplay = new ObservableCollection<ProductsDisplay>(meal.GetProductsContainingAllergen(SearchBar, false));
                    }
                    else
                    {
                        ProductsDisplay = new ObservableCollection<ProductsDisplay>(meal.GetProductsContainingAllergen(SearchBar, true));
                    }
                }
            }
        }

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
