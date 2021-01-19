using Restaurant.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Restaurant.Helps
{
    public enum OrderStatus
    {
        Registerd,
        Preparing,
        Left,
        Delivered,
        Canceled
    }

    class Statuses
    {
        public List<OrderStatus> OrderStatuses = new List<OrderStatus>();

        public Statuses()
        {
            OrderStatuses.Add(OrderStatus.Registerd);
            OrderStatuses.Add(OrderStatus.Preparing);
            OrderStatuses.Add(OrderStatus.Left);
            OrderStatuses.Add(OrderStatus.Delivered);
            OrderStatuses.Add(OrderStatus.Canceled);
        }

        public OrderStatus GetNextStatus(string orderStatus)
        {
            bool ok = false;
            Regex regex = new Regex(@"\s");
            orderStatus = regex.Replace(orderStatus, "");
            foreach (var status in OrderStatuses)
            {
                if (status == OrderStatus.Canceled)
                {
                    break;
                }
                if (ok == true)
                {
                    return status;
                }
                if (status.ToString() == orderStatus)
                {
                    ok = true;
                }
            }
            return OrderStatus.Registerd;
        }
    }
}

