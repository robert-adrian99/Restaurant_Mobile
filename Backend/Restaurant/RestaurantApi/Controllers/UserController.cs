using System;
using System.Web.Http;
using Restaurant.Models.BusinessLogicLayer;

namespace RestaurantApi.Controllers
{
    public class UserController : ApiController
    {
        private UserBLL userBLL;
        public UserController()
        {
            userBLL = new UserBLL();
        }

        [HttpGet]
        public IHttpActionResult SignIn(string email, string password)
        {
            try
            {
                userBLL.SignIn(email, password);
                return Ok("Logged in successfully!");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult AddUser([FromBody] dynamic user)
        {
            try
            {
                string firstName = user["FirstName"];
                string lastName = user["LastName"];
                string email = user["Email"];
                string phone = user["Phone"];
                string address = user["Address"];
                string password = user["Password"];

                userBLL.SignUp(firstName, lastName, email, phone, address, password);

                return Ok("User added successfully!");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}