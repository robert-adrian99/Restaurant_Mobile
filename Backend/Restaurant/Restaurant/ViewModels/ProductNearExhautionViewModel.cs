using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Restaurant.Helps;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.Entity;

namespace Restaurant.ViewModels
{
    public class ProductNearExhautionViewModel : NotifyPropertyChangedHelp
    {
        public ProductNearExhautionViewModel()
        {
            CategoryBLL category = new CategoryBLL();
            Categories = new ObservableCollection<string>(category.GetCategories());
            for (int index = 0; index < Categories.Count; index++)
            {
                Regex regex = new Regex(@"\s*$");
                Categories[index] = regex.Replace(Categories[index], "");
                Categories[index] = Categories[index].ToUpper();
            }
            SelectedItemCombobox = Categories.First();
            ProductsDisplay = new ObservableCollection<ProductsDisplay>(meal.GetProductsWithTotalQuantity(SelectedItemCombobox));
        }

        #region DataMembers
        MealBLL meal = new MealBLL();

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

        private ObservableCollection<string> categories;
        public ObservableCollection<string> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
                NotifyPropertyChanged("Categories");
            }
        }

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
                ProductsDisplay = new ObservableCollection<ProductsDisplay>(meal.GetProductsWithTotalQuantity(selectedItemCombobox));
                NotifyPropertyChanged("SelectedItemCombobox");
            }
        }
        #endregion
    }
}
