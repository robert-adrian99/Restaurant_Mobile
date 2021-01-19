using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
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
    public class EmployeeViewOrdersViewModel : NotifyPropertyChangedHelp
    {
        #region DataMembers
        private OrderBLL order = new OrderBLL();

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
                if (SelectedItemCombobox.Contains("ACTIVE"))
                {
                    OrdersDisplays = new ObservableCollection<OrdersDisplay>(order.GetActiveOrders());
                }
                else
                {
                    OrdersDisplays = new ObservableCollection<OrdersDisplay>(order.GetAllOrders());
                }
                NotifyPropertyChanged("SelectedItemCombobox");
            }
        }

        private OrdersDisplay selectedItemList;
        public OrdersDisplay SelectedItemList
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

        private ObservableCollection<OrdersDisplay> ordersDisplays;
        public ObservableCollection<OrdersDisplay> OrdersDisplays
        {
            get
            {
                return ordersDisplays;
            }
            set
            {
                ordersDisplays = value;
                NotifyPropertyChanged("OrdersDisplays");
            }
        }
        #endregion

        #region CommandMembers
        private bool CanExecuteCommand { get; set; } = false;
        private ICommand changeStatusCommand;
        public ICommand ChangeStatusCommand
        {
            get
            {
                if (changeStatusCommand == null)
                {
                    changeStatusCommand = new RelayCommand(ChangeStatus, param => CanExecuteCommand);
                }
                return changeStatusCommand;
            }
        }

        public void ChangeStatus(object param)
        {
            if (!SelectedItemCombobox.Contains("ACTIVE"))
            {
                MessageBox.Show("Select an active order!");
            }
            else
            {
                if (SelectedItemList == null)
                {
                    MessageBox.Show("Select an order!");
                }
                else
                {
                    Statuses statuses = new Statuses();
                    OrderStatus nextOrderStatus = statuses.GetNextStatus(SelectedItemList.Status);
                    if (MessageBox.Show("Do you want to change the status from " + SelectedItemList.Status + " to " +  nextOrderStatus, "Change status", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (order.UpdateStatusByOrder(SelectedItemList, nextOrderStatus))
                        {
                            OrdersDisplays = new ObservableCollection<OrdersDisplay>(order.GetActiveOrders());
                            MessageBox.Show("Status changed");
                        }
                        else
                        {
                            MessageBox.Show("Something went wrong!");
                        }
                    }
                }
            }
        }

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
                MessageBox.Show("Select an order");
            }
            else
            {
                OrderDetails orderDetails = order.GetOrderDetails(SelectedItemList);
                OrderDetailsWindow orderDetailsWindow = new OrderDetailsWindow();
                OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel(orderDetails);
                orderDetailsWindow.DataContext = orderDetailsViewModel;
                orderDetailsWindow.ShowDialog();
            }
        }
        #endregion
    }
}
