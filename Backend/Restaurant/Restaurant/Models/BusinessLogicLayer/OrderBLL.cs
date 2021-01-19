using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Helps;
using Restaurant.Models.Entity;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class OrderBLL
    {
        private RestaurantEntities restaurantEntities = new RestaurantEntities();
        private User activeUser = new User();

        public OrderBLL()
        {
            activeUser = (from user in restaurantEntities.User
                          where user.Active
                          select user).First();
        }

        public bool AddOrder(double price, List<ProductsInCart> productsInCart)
        {
            foreach (var productInCart in productsInCart)
            {
                try
                {
                    var productQuery = (from product in restaurantEntities.Product
                                        where product.Name.Equals(productInCart.Name)
                                        select product).First();

                    if (productQuery.Quantity > productQuery.TotalQuantity)
                    {
                        return false;
                    }
                }
                catch
                {
                    var menuQuery = (from menu in restaurantEntities.Menu
                                     join menu_product in restaurantEntities.Menu_Product
                                     on menu.MenuID equals menu_product.MenuID
                                     join product in restaurantEntities.Product
                                     on menu_product.ProductID equals product.ProductID
                                     where menu.Name.Equals(productInCart.Name)
                                     select new { product.TotalQuantity, menu_product.Quantity }).First();

                    if (menuQuery.Quantity > menuQuery.TotalQuantity)
                    {
                        return false;
                    }
                }
            }

            Order newOrder = new Order()
            {
                OrderNumber = Properties.Settings.Default.OrderNumber++,
                UserID = activeUser.UserID,
                Status = OrderStatus.Registerd.ToString(),
                Date = DateTime.Now,
                Price = price
            };
            Properties.Settings.Default.Save();

            restaurantEntities.Order.Add(newOrder);

            foreach (var product in productsInCart)
            {
                if (product.ProductType == ProductTypeEnum.Product)
                {
                    for (int numberOfProducts = 0; numberOfProducts < product.Pieces; numberOfProducts++)
                    {
                        restaurantEntities.Order_Product.Add(new Order_Product
                        {
                            OrderID = newOrder.OrderID,
                            ProductID = (from productEntity in restaurantEntities.Product
                                         where productEntity.Name.Equals(product.Name)
                                         select productEntity.ProductID).First()
                        });
                    }
                }
                else
                {
                    for (int numberOfMenus = 0; numberOfMenus < product.Pieces; numberOfMenus++)
                    {
                        restaurantEntities.Order_Menu.Add(new Order_Menu
                        {
                            OrderID = newOrder.OrderID,
                            MenuID = (from menu in restaurantEntities.Menu
                                      where menu.Name.Equals(product.Name)
                                      select menu.MenuID).First()
                        });
                    }
                }
            }

            restaurantEntities.SaveChanges();
            return true;
        }

        public List<OrdersDisplay> GetActiveOrdersByUser()
        {
            var activeOrders = (from order in restaurantEntities.Order
                                where order.UserID.Equals(activeUser.UserID)
                                && !order.Status.Equals(OrderStatus.Canceled.ToString())
                                && !order.Status.Equals(OrderStatus.Delivered.ToString())
                                select new OrdersDisplay
                                {
                                    OrderNumber = order.OrderNumber,
                                    Price = order.Price,
                                    Date = order.Date,
                                    Status = order.Status
                                }).ToList();

            foreach (var order in activeOrders)
            {
                order.EstimatedDate = order.Date.AddMinutes(Constants.DeliveryTime);
            }
            return activeOrders;
        }

        public List<OrdersDisplay> GetAllDeliveredOrCanceledOrders()
        {
            List<OrdersDisplay> orders = new List<OrdersDisplay>();

            var allOrders = (from order in restaurantEntities.Order
                             where order.UserID.Equals(activeUser.UserID)
                             select new OrdersDisplay
                             {
                                 OrderNumber = order.OrderNumber,
                                 Price = order.Price,
                                 Date = order.Date,
                                 Status = order.Status
                             }).ToList();

            foreach (var order in allOrders)
            {
                if (order.Status.Contains(OrderStatus.Canceled.ToString()) || order.Status.Contains(OrderStatus.Delivered.ToString()))
                {
                    orders.Add(order);
                }
            }

            foreach (var order in orders)
            {
                order.EstimatedDate = order.Date.AddMinutes(Constants.DeliveryTime);
            }
            return orders;
        }

        public List<OrdersDisplay> GetAllOrdersInTimeInterval()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.Subtract(TimeSpan.FromDays(Constants.DiscountTime));

            var allOrders = (from order in restaurantEntities.Order
                             where order.UserID.Equals(activeUser.UserID)
                             && order.Date > dateTime
                             select new OrdersDisplay
                             {
                                 OrderNumber = order.OrderNumber,
                                 Price = order.Price,
                                 Date = order.Date,
                                 Status = order.Status
                             }).ToList();

            foreach (var order in allOrders)
            {
                order.EstimatedDate = order.Date.AddMinutes(Constants.DeliveryTime);
            }

            return allOrders;
        }

        public void CancelOrder(OrdersDisplay activeOrder)
        {
            var orderQuery = (from order in restaurantEntities.Order
                              where order.OrderNumber.Equals(activeOrder.OrderNumber)
                              select order).First();

            orderQuery.Status = OrderStatus.Canceled.ToString();

            restaurantEntities.Order.Attach(orderQuery);
            restaurantEntities.Entry(orderQuery).Property(x => x.Status).IsModified = true;
            restaurantEntities.SaveChanges();
        }

        public List<OrdersDisplay> GetActiveOrders()
        {
            List<OrdersDisplay> orders = new List<OrdersDisplay>();

            var allOrders = (from order in restaurantEntities.Order
                             orderby order.Date descending
                             select new OrdersDisplay
                             {
                                 OrderNumber = order.OrderNumber,
                                 Price = order.Price,
                                 Date = order.Date,
                                 Status = order.Status
                             }).ToList();

            foreach (var order in allOrders)
            {
                if (!order.Status.Contains(OrderStatus.Canceled.ToString())
                    && !order.Status.Contains(OrderStatus.Delivered.ToString()))
                {
                    orders.Add(order);
                }
            }

            return orders;
        }

        public List<OrdersDisplay> GetAllOrders()
        {
            return (from order in restaurantEntities.Order
                    orderby order.Date descending
                    select new OrdersDisplay
                    {
                        OrderNumber = order.OrderNumber,
                        Price = order.Price,
                        Date = order.Date,
                        Status = order.Status
                    }).ToList();
        }

        public bool UpdateStatusByOrder(OrdersDisplay orderDisplay, OrderStatus orderStatus)
        {
            try
            {
                var orderQuery = (from order in restaurantEntities.Order
                                  where order.OrderNumber.Equals(orderDisplay.OrderNumber)
                                  select order).First();

                orderQuery.Status = orderStatus.ToString();

                if (orderStatus == OrderStatus.Delivered)
                {
                    var productQuery = (from product in restaurantEntities.Product
                                        join order_product in restaurantEntities.Order_Product
                                        on product.ProductID equals order_product.ProductID
                                        join order in restaurantEntities.Order
                                        on order_product.OrderID equals order.OrderID
                                        where order.OrderNumber.Equals(orderDisplay.OrderNumber)
                                        select product).ToList();

                    foreach (var product in productQuery)
                    {
                        product.TotalQuantity -= product.Quantity;
                        restaurantEntities.Product.Attach(product);
                        restaurantEntities.Entry(product).Property(x => x.TotalQuantity).IsModified = true;
                    }

                    var menuQuery = (from menu in restaurantEntities.Menu
                                     join order_menu in restaurantEntities.Order_Menu
                                     on menu.MenuID equals order_menu.MenuID
                                     join order in restaurantEntities.Order
                                     on order_menu.OrderID equals order.OrderID
                                     join menu_product in restaurantEntities.Menu_Product
                                     on menu.MenuID equals menu_product.MenuID
                                     join product in restaurantEntities.Product
                                     on menu_product.ProductID equals product.ProductID
                                     where order.OrderNumber.Equals(orderDisplay.OrderNumber)
                                     select product).ToList();

                    foreach (var product in menuQuery)
                    {
                        //for(int pice = 0; piece < )
                        product.TotalQuantity -= product.Quantity;
                        restaurantEntities.Product.Attach(product);
                        restaurantEntities.Entry(product).Property(x => x.TotalQuantity).IsModified = true;
                    }

                }

                restaurantEntities.Order.Attach(orderQuery);
                restaurantEntities.Entry(orderQuery).Property(x => x.Status).IsModified = true;

                restaurantEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public OrderDetails GetOrderDetails(OrdersDisplay ordersDisplay)
        {

            var productUserOrderQuery = (from order in restaurantEntities.Order
                                         join order_product in restaurantEntities.Order_Product
                                         on order.OrderID equals order_product.OrderID
                                         join product in restaurantEntities.Product
                                         on order_product.ProductID equals product.ProductID
                                         join user in restaurantEntities.User
                                         on order.UserID equals user.UserID
                                         where order.OrderNumber.Equals(ordersDisplay.OrderNumber)
                                         orderby order.OrderNumber
                                         select new { order, product, user }).ToList();

            var menuUserOrderQuery = (from order in restaurantEntities.Order
                                      join order_menu in restaurantEntities.Order_Menu
                                      on order.OrderID equals order_menu.OrderID
                                      join menu in restaurantEntities.Menu
                                      on order_menu.MenuID equals menu.MenuID
                                      join user in restaurantEntities.User
                                      on order.UserID equals user.UserID
                                      where order.OrderNumber.Equals(ordersDisplay.OrderNumber)
                                      orderby order.OrderNumber
                                      select new { order, menu, user }).ToList();

            var prod = productUserOrderQuery.Count != 0 ? productUserOrderQuery[0] : null;
            var prodFin = prod == null ? menuUserOrderQuery[0] : null;

            List<ProductsDetails> productQuantity = new List<ProductsDetails>();

            foreach (var item in productUserOrderQuery)
            {
                bool ok = false;
                for (int index = 0; index < productQuantity.Count; index++)
                {
                    if (productQuantity[index].Name == item.product.Name)
                    {
                        productQuantity[index].Quantity += 1;
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    productQuantity.Add(new ProductsDetails { Name = item.product.Name, Quantity = 1 });
                }
            }

            foreach (var item in menuUserOrderQuery)
            {

                bool ok = false;
                for (int index = 0; index < productQuantity.Count; index++)
                {
                    if (productQuantity[index].Name == item.menu.Name)
                    {
                        productQuantity[index].Quantity += 1;
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    productQuantity.Add(new ProductsDetails { Name = item.menu.Name, Quantity = 1 });
                }
            }

            OrderDetails orderDetails = new OrderDetails
            {
                OrderDetail = prodFin == null ? prod.order : prodFin.order,
                EstimatedDate = prodFin == null ? prod.order.Date.AddMinutes(Constants.DeliveryTime) : prodFin.order.Date.AddMinutes(Constants.DeliveryTime),
                UserDetail = prodFin == null ? prod.user : prodFin.user,
                Products = productQuantity
            };

            return orderDetails;
        }
    }
}
