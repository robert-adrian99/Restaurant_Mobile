using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Restaurant.Helps;
using Restaurant.Models.Entity;

namespace Restaurant.ViewModels
{
    public class OrderDetailsViewModel : NotifyPropertyChangedHelp
    {
        public OrderDetailsViewModel(OrderDetails orderDetails)
        {
            order = orderDetails;
        }

        #region DataMembers
        private OrderDetails order;

        public int OrderNumber
        {
            get
            {
                return order.OrderDetail.OrderNumber;
            }
        }

        public DateTime Date
        {
            get
            {
                return order.OrderDetail.Date;
            }
        }

        public DateTime EstimatedDate
        {
            get
            {
                return order.EstimatedDate;
            }
        }

        public double Price
        {
            get
            {
                return order.OrderDetail.Price;
            }
        }

        public string Status
        {
            get
            {
                return order.OrderDetail.Status;
            }
        }

        public string UserName
        {
            get
            {
                Regex regex = new Regex(@"\s");
                return regex.Replace(order.UserDetail.FirstName, "") + " " + regex.Replace(order.UserDetail.LastName, "");
            }
        }

        public string Phone
        {
            get
            {
                return order.UserDetail.Phone;
            }
        }

        public string Address
        {
            get
            {
                return order.UserDetail.Address;
            }
        }

        public ObservableCollection<ProductsDetails> Products
        {
            get
            {
                return new ObservableCollection<ProductsDetails>(order.Products);
            }
        }
        #endregion
    }
}
