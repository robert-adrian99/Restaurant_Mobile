using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Entity
{
    public enum ProductTypeEnum
    {
        Product,
        Menu
    }

    public class ProductsDisplay
    {
        public ProductTypeEnum ProductType { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public byte[] Image1 { get; set; }
        public byte[] Image2 { get; set; }
        public byte[] Image3 { get; set; }
        public List<string> Allergens { get; set; }
        public List<Product> Products { get; set; }
    }
}
