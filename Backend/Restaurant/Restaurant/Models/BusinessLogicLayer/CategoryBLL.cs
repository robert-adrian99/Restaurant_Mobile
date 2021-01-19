using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class CategoryBLL
    {
        private readonly RestaurantEntities restaurantEntities;

        public CategoryBLL()
        {
            restaurantEntities = new RestaurantEntities();
        }

        public List<string> GetCategories()
        {
            var query = (from category in restaurantEntities.Category
                         select category.Name)?.ToList();
            return query;
        }
    }
}
