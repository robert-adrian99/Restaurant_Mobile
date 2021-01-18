using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Restaurant.Helps;
using Restaurant.Models;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.Entity;

namespace Restaurant.ViewModels
{
    public class SeeCartViewModel : NotifyPropertyChangedHelp
    {

        public SeeCartViewModel(List<ProductsDisplay> productsDisplays)
        {
            foreach (var product in productsDisplays)
            {
                bool ok = true;
                foreach (var product1 in ProductsInCarts)
                {
                    if (product.Name == product1.Name)
                    {
                        product1.Pieces = product1.Pieces + 1;
                        product1.Price = product.Price;
                        product1.TotalPrice += product1.Price;
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    ProductsInCarts.Add(new ProductsInCart()
                    {
                        Name = product.Name,
                        Pieces = 1,
                        Price = product.Price,
                        TotalPrice = product.Price,
                        ProductType = product.ProductType
                    });

                }
            }

            double sum = 0;
            foreach (var product in ProductsInCarts)
            {
                sum += product.Price * product.Pieces;
            }

            Subtotal = Math.Round(sum, 2);

            if (Subtotal > Constants.OrderPriceGreaterThanForDiscount)
            {
                Discount = Math.Round(Constants.OrderDiscount / 100 * Subtotal, 2);
            }
            else
            {
                OrderBLL order = new OrderBLL();
                if (order.GetAllOrdersInTimeInterval().Count > Constants.NumberMinOrders)
                {
                    Discount = Math.Round(Constants.OrderDiscount / 100 * Subtotal, 2);
                }
            }

            if (Subtotal < Constants.OrderPriceGreaterThanForDelivery)
            {
                DeliveryCost = ProductsInCarts.Count != 0 ? Math.Round(Constants.DeliveryCost, 2) : 0;
            }

            Total = Math.Round(Subtotal - Discount + DeliveryCost, 2);
        }

        #region DataMembers
        private ObservableCollection<ProductsInCart> productsInCarts = new ObservableCollection<ProductsInCart>();
        public ObservableCollection<ProductsInCart> ProductsInCarts
        {
            get
            {
                return productsInCarts;
            }
            set
            {
                productsInCarts = value;
                NotifyPropertyChanged("ProductsInCarts");
            }
        }

        private double subtotal;
        public double Subtotal
        {
            get
            {
                return subtotal;
            }
            set
            {
                subtotal = value;
                NotifyPropertyChanged("Subtotal");
            }
        }

        private double deliveryCost;
        public double DeliveryCost
        {
            get
            {
                return deliveryCost;
            }
            set
            {
                deliveryCost = value;
                NotifyPropertyChanged("DeliveryCost");
            }
        }

        private double discount;
        public double Discount
        {
            get
            {
                return discount;
            }
            set
            {
                discount = value;
                NotifyPropertyChanged("Discount");
            }
        }

        private double total;
        public double Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                NotifyPropertyChanged("Total");
            }
        }

        private ProductsInCart selectedItem;
        public ProductsInCart SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                CanExecuteCommand = true;
                NotifyPropertyChanged("SelectedItem");
            }
        }
        #endregion

        #region CommandMembers
        private bool CanExecuteCommand { get; set; } = false;
        private ICommand deleteItemCommand;
        public ICommand DeleteItemCommand
        {
            get
            {
                if (deleteItemCommand == null)
                {
                    deleteItemCommand = new RelayCommand(DeleteItem, param => CanExecuteCommand);
                }
                return deleteItemCommand;
            }
        }

        public void DeleteItem(object param)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Select an item");
            }
            else
            {
                foreach (var item in ProductsInCarts)
                {
                    if (item.Name == SelectedItem.Name)
                    {
                        Subtotal = Math.Round(Subtotal - item.Price, 2);

                        bool ok = false;
                        if (item.Pieces > 1)
                        {
                            item.Pieces--;
                            item.TotalPrice -= item.Price;
                        }
                        else
                        {
                            ProductsInCarts.Remove(item);
                            ok = true;
                        }

                        if (Subtotal > Constants.OrderPriceGreaterThanForDiscount)
                        {
                            Discount = Math.Round(Constants.OrderDiscount / 100 * Subtotal, 2);
                        }

                        if (Subtotal < Constants.OrderPriceGreaterThanForDelivery)
                        {
                            DeliveryCost = ProductsInCarts.Count != 0 ? Math.Round(Constants.DeliveryCost, 2) : 0;
                        }

                        Total = Math.Round(Subtotal - Discount + DeliveryCost, 2);

                        if (ok)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private ICommand placeOrderCommand;
        public ICommand PlaceOrderCommand
        {
            get
            {
                if (placeOrderCommand == null)
                {
                    placeOrderCommand = new RelayCommand(PlaceOrder);
                }
                return placeOrderCommand;
            }
        }

        public void PlaceOrder(object param)
        {
            if (ProductsInCarts.Count == 0)
            {
                MessageBox.Show("Your cart is empty!");
            }
            else
            {
                if (MessageBox.Show("Your order worth $" + Total + " will be placed!\n\nAre you sure you want to place this order?", "Place order", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    OrderBLL orderBLL = new OrderBLL();
                    if (orderBLL.AddOrder(Total, productsInCarts.ToList()))
                    {
                        MessageBox.Show("Order placed successfully");
                    }
                    else
                    {
                        MessageBox.Show("Not enough quantity in store!");
                    }
                }
            }
        }
        #endregion
    }
}
