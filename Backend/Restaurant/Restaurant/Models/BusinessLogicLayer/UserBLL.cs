using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class UserBLL
    {
        private RestaurantEntities restaurantEntities = new RestaurantEntities();

        public dynamic GetUsers()
        {
            try
            {
                return (from user in restaurantEntities.User
                        select new { user.FirstName, user.LastName, user.Email, user.Address, user.Password, user.Active }).ToList();
            }
            catch
            {
                throw;
            }
        }

        public void SignIn(string email, string password)
        {
            try
            {
                var userQuery = (from user in restaurantEntities.User
                                 where user.Email.Equals(email) && user.Password.Equals(password)
                                 select user).First();

                var query = (from user in restaurantEntities.User
                             select user)?.ToList();

                foreach (var userInList in query)
                {
                    userInList.Active = false;

                    restaurantEntities.User.Attach(userInList);
                    restaurantEntities.Entry(userInList).Property(x => x.Active).IsModified = true;
                    restaurantEntities.SaveChanges();
                }

                userQuery.Active = true;

                restaurantEntities.User.Attach(userQuery);
                restaurantEntities.Entry(userQuery).Property(x => x.Active).IsModified = true;
                restaurantEntities.SaveChanges();
            }
            catch
            {
                throw new Exception();
            }
        }

        public bool SignUp(string firstName, string lastName, string email, string phone, string address, string password)
        {

            var query = (from user in restaurantEntities.User
                         select user)?.ToList();

            foreach (var userInList in query)
            {
                if (userInList.Email.Contains(email))
                {
                    return false;
                }
            }

            foreach (var userInList in query)
            {
                userInList.Active = false;

                restaurantEntities.User.Attach(userInList);
                restaurantEntities.Entry(userInList).Property(x => x.Active).IsModified = true;
                restaurantEntities.SaveChanges();
            }

            User newUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Address = address,
                Password = password,
                Active = true
            };

            restaurantEntities.User.Add(newUser);
            restaurantEntities.SaveChanges();
            return true;
        }
    }
}
