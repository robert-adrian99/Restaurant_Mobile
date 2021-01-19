using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Entity
{
    public class ProductsDetails
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderDetails
    {
        public Order OrderDetail { get; set; }
        public DateTime EstimatedDate { get; set; }
        public User UserDetail { get; set; }
        public List<ProductsDetails> Products { get; set; }
    }
}
