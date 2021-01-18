using System;
using System.Collections.Generic;
using System.Web.Http;
using Restaurant.Models.BusinessLogicLayer;
using Restaurant.Models.Entity;

namespace RestaurantApi.Controllers
{
    public class OrderController : ApiController
    {
        private OrderBLL orderBLL;
        public OrderController()
        {
            orderBLL = new OrderBLL();
        }

        public IHttpActionResult AddOrder([FromBody] dynamic order)
        {
            try
            {
                double price = order["Price"];
                List<ProductsInCart> products = order["Products"];
                orderBLL.AddOrder(price, products);

                return Ok("Order added successfully!");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}