using Restaurant.Models.BusinessLogicLayer;
using System;
using System.Web.Http;

namespace RestaurantApi.Controllers
{
    public class CategoryController : ApiController
    {
        private CategoryBLL categoryBLL;

        public CategoryController()
        {
            categoryBLL = new CategoryBLL();
        }

        public IHttpActionResult GetCategories()
        {
            try
            {
                var categories = categoryBLL.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
