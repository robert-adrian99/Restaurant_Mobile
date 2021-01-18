using System;
using System.Web.Http;
using Restaurant.Models.BusinessLogicLayer;

namespace RestaurantApi.Controllers
{
    public class ProductController : ApiController
    {
        private MealBLL mealBLL;
        public ProductController()
        {
            mealBLL = new MealBLL();
        }

        public IHttpActionResult GetProductsByCategory(string category)
        {
            try
            {
                var products = mealBLL.GetProductsByCategory(category);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult GetProductDetails(string productName)
        {
            try
            {
                var product = mealBLL.GetProductDetails(productName);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}