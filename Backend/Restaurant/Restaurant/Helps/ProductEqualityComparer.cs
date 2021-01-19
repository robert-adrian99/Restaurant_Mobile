using Restaurant.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Helps
{
    class ProductEqualityComparer : IEqualityComparer<ProductsDisplay>
    {
        public bool Equals(ProductsDisplay product1, ProductsDisplay product2)
        {
            if (product1 == null && product2 == null)
                return true;
            else if (product1 == null || product2 == null)
                return false;
            else if (product1.Name == product2.Name)
                return true;
            else
                return false;
        }

        public int GetHashCode(ProductsDisplay product)
        {
            int hCode = 0;
            return hCode.GetHashCode();
        }
    }
}
