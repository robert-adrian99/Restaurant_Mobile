using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Entity
{
    public class OrdersDisplay
    {
        public int OrderNumber { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedDate { get; set; }
    }
}
