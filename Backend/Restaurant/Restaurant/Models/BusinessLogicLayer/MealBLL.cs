using Restaurant.Helps;
using Restaurant.Models.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Restaurant.Models.BusinessLogicLayer
{
    public class MealBLL
    {
        private RestaurantEntities restaurantEntities = new RestaurantEntities();
        private ProductEqualityComparer productEqualityComparer = new ProductEqualityComparer();

        private double MenuPriceAfterDiscount(double price)
        {
            return price - Constants.MenuDiscount / 100 * price;
        }

        public List<ProductsDisplay> GetProductsContainingInName(string productName, bool containing)
        {
            if (containing)
            {
                var query = (from product in restaurantEntities.Product
                             where product.Name.Contains(productName)
                             select new ProductsDisplay
                             {
                                 Name = product.Name,
                                 Quantity = product.Quantity,
                                 Price = product.Price,
                                 CategoryName = product.Category.Name,
                                 Image1 = product.Image1,
                                 Image2 = product.Image2,
                                 Image3 = product.Image3,
                                 ProductType = ProductTypeEnum.Product
                             }).ToList();

                query = query.Distinct(productEqualityComparer).ToList();

                foreach (var product in query)
                {
                    product.Allergens = restaurantEntities.GetAllergensByProduct(product.Name).ToList();
                }

                var query1 = (from menu in restaurantEntities.Menu
                              where menu.Name.Contains(productName)
                              select new ProductsDisplay
                              {
                                  Name = menu.Name,
                                  Quantity = (int)(from menu_product in restaurantEntities.Menu_Product
                                                   where menu.MenuID.Equals(menu_product.MenuID)
                                                   select menu_product.Quantity).Sum(),
                                  Price = (from product in restaurantEntities.Product
                                           join menu_product in restaurantEntities.Menu_Product
                                           on product.ProductID equals menu_product.ProductID
                                           where menu.MenuID.Equals(menu_product.MenuID)
                                           select product.Price).Sum(),
                                  CategoryName = menu.Category.Name,
                                  Image1 = menu.Image1,
                                  Image2 = menu.Image2,
                                  Image3 = menu.Image3,
                                  ProductType = ProductTypeEnum.Menu
                              }).ToList();

                foreach (var product in query1)
                {
                    product.Allergens = restaurantEntities.GetAllergensByMenu(product.Name).ToList();
                    var list = restaurantEntities.GetProductsByMenu(product.Name).ToList();
                    product.Products = new List<Product>();
                    foreach (var product1 in list)
                    {
                        product.Products.Add(new Product()
                        {
                            ProductID = product1.ProductID,
                            Name = product1.Name,
                            Quantity = (int)product1.Quantity,
                            Price = product1.Price
                        });
                    }
                }

                query1 = query1.Distinct(productEqualityComparer).ToList();

                query.AddRange(query1);
                return query;
            }
            else
            {
                var query = (from product in restaurantEntities.Product
                             where !product.Name.Contains(productName)
                             select new ProductsDisplay
                             {
                                 Name = product.Name,
                                 Quantity = product.Quantity,
                                 Price = product.Price,
                                 CategoryName = product.Category.Name,
                                 Image1 = product.Image1,
                                 Image2 = product.Image2,
                                 Image3 = product.Image3,
                                 ProductType = ProductTypeEnum.Product
                             }).ToList();

                query = query.Distinct(productEqualityComparer).ToList();

                foreach (var product in query)
                {
                    product.Allergens = restaurantEntities.GetAllergensByProduct(product.Name).ToList();
                }

                var query1 = (from menu in restaurantEntities.Menu
                              where !menu.Name.Contains(productName)
                              select new ProductsDisplay
                              {
                                  Name = menu.Name,
                                  Quantity = (int)(from menu_product in restaurantEntities.Menu_Product
                                                   where menu.MenuID.Equals(menu_product.MenuID)
                                                   select menu_product.Quantity).Sum(),
                                  Price = (from product in restaurantEntities.Product
                                           join menu_product in restaurantEntities.Menu_Product
                                           on product.ProductID equals menu_product.ProductID
                                           where menu.MenuID.Equals(menu_product.MenuID)
                                           select product.Price).Sum(),
                                  CategoryName = menu.Category.Name,
                                  Image1 = menu.Image1,
                                  Image2 = menu.Image2,
                                  Image3 = menu.Image3,
                                  ProductType = ProductTypeEnum.Menu
                              }).ToList();

                foreach (var product in query1)
                {
                    product.Allergens = restaurantEntities.GetAllergensByMenu(product.Name).ToList();
                    var list = restaurantEntities.GetProductsByMenu(product.Name).ToList();
                    product.Products = new List<Product>();
                    foreach (var product1 in list)
                    {
                        product.Products.Add(new Product()
                        {
                            ProductID = product1.ProductID,
                            Name = product1.Name,
                            Quantity = (int)product1.Quantity,
                            Price = product1.Price
                        });
                    }
                }

                query1 = query1.Distinct(productEqualityComparer).ToList();

                query.AddRange(query1);
                return query;
            }
        }

        public List<ProductsDisplay> GetProductsContainingAllergen(string allergenName, bool containing)
        {
            if (containing)
            {
                var query = (from product in restaurantEntities.Product
                             join product_allergen in restaurantEntities.Product_Allergen
                             on product.ProductID equals product_allergen.AllergenID
                             join allergen in restaurantEntities.Allergen
                             on product_allergen.AllergenID equals allergen.AllergenID
                             where allergen.Name.Contains(allergenName)
                             select new ProductsDisplay
                             {
                                 Name = product.Name,
                                 Quantity = product.Quantity,
                                 Price = product.Price,
                                 CategoryName = product.Category.Name,
                                 Image1 = product.Image1,
                                 Image2 = product.Image2,
                                 Image3 = product.Image3,
                                 ProductType = ProductTypeEnum.Product
                             }).ToList();

                query = query.Distinct(productEqualityComparer).ToList();

                foreach (var product in query)
                {
                    product.Allergens = restaurantEntities.GetAllergensByProduct(product.Name).ToList();
                }

                var query1 = (from menu in restaurantEntities.Menu
                              join menu_product in restaurantEntities.Menu_Product
                              on menu.MenuID equals menu_product.MenuID
                              join product in restaurantEntities.Product
                              on menu_product.ProductID equals product.ProductID
                              join product_allergen in restaurantEntities.Product_Allergen
                              on product.ProductID equals product_allergen.ProductID
                              join allergen in restaurantEntities.Allergen
                              on product_allergen.AllergenID equals allergen.AllergenID
                              where allergen.Name.Contains(allergenName)
                              select new ProductsDisplay
                              {
                                  Name = menu.Name,
                                  Quantity = (int)(from menu_product in restaurantEntities.Menu_Product
                                                   where menu.MenuID.Equals(menu_product.MenuID)
                                                   select menu_product.Quantity).Sum(),
                                  Price = (from product in restaurantEntities.Product
                                           join menu_product in restaurantEntities.Menu_Product
                                           on product.ProductID equals menu_product.ProductID
                                           where menu.MenuID.Equals(menu_product.MenuID)
                                           select product.Price).Sum(),
                                  CategoryName = menu.Category.Name,
                                  Image1 = menu.Image1,
                                  Image2 = menu.Image2,
                                  Image3 = menu.Image3,
                                  ProductType = ProductTypeEnum.Menu
                              }).ToList();

                foreach (var product in query1)
                {
                    product.Allergens = restaurantEntities.GetAllergensByMenu(product.Name).ToList();
                    var list = restaurantEntities.GetProductsByMenu(product.Name).ToList();
                    product.Products = new List<Product>();
                    foreach (var product1 in list)
                    {
                        product.Products.Add(new Product()
                        {
                            ProductID = product1.ProductID,
                            Name = product1.Name,
                            Quantity = (int)product1.Quantity,
                            Price = product1.Price
                        });
                    }
                }

                query1 = query1.Distinct(productEqualityComparer).ToList();

                query.AddRange(query1);
                return query;
            }
            else
            {
                var query = (from product in restaurantEntities.Product
                             select new ProductsDisplay
                             {
                                 Name = product.Name,
                                 Quantity = product.Quantity,
                                 Price = product.Price,
                                 CategoryName = product.Category.Name,
                                 Image1 = product.Image1,
                                 Image2 = product.Image2,
                                 Image3 = product.Image3,
                                 ProductType = ProductTypeEnum.Product
                             }).ToList();

                query = query.Distinct(productEqualityComparer).ToList();

                List<ProductsDisplay> products = new List<ProductsDisplay>();
                foreach (var product in query)
                {
                    product.Allergens = restaurantEntities.GetAllergensByProduct(product.Name).ToList();
                    bool ok = true;
                    foreach (var allergen in product.Allergens)
                    {
                        if (allergen.ToLower().Contains(allergenName.ToLower()))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        products.Add(product);
                    }
                }

                var query1 = (from menu in restaurantEntities.Menu
                              select new ProductsDisplay
                              {
                                  Name = menu.Name,
                                  Quantity = (int)(from menu_product in restaurantEntities.Menu_Product
                                                   where menu.MenuID.Equals(menu_product.MenuID)
                                                   select menu_product.Quantity).Sum(),
                                  Price = (from product in restaurantEntities.Product
                                           join menu_product in restaurantEntities.Menu_Product
                                           on product.ProductID equals menu_product.ProductID
                                           where menu.MenuID.Equals(menu_product.MenuID)
                                           select product.Price).Sum(),
                                  CategoryName = menu.Category.Name,
                                  Image1 = menu.Image1,
                                  Image2 = menu.Image2,
                                  Image3 = menu.Image3,
                                  ProductType = ProductTypeEnum.Menu
                              }).ToList();

                query1 = query1.Distinct(productEqualityComparer).ToList();

                List<ProductsDisplay> menus = new List<ProductsDisplay>();
                foreach (var menu in query1)
                {
                    menu.Allergens = restaurantEntities.GetAllergensByMenu(menu.Name).ToList();
                    var list = restaurantEntities.GetProductsByMenu(menu.Name).ToList();
                    menu.Products = new List<Product>();
                    foreach (var product1 in list)
                    {
                        menu.Products.Add(new Product()
                        {
                            ProductID = product1.ProductID,
                            Name = product1.Name,
                            Quantity = (int)product1.Quantity,
                            Price = product1.Price
                        });
                    }
                    bool ok = true;
                    foreach (var allergen in menu.Allergens)
                    {
                        if (allergen.ToLower().Contains(allergenName.ToLower()))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        menus.Add(menu);
                    }
                }

                products.AddRange(menus);
                return products;
            }
        }

        public List<ProductsDisplay> GetProductsByCategory(string category)
        {
            List<ProductsDisplay> productsDisplays = new List<ProductsDisplay>();

            var query = (from product in restaurantEntities.Product
                         where product.Category.Name.Equals(category)
                         select new ProductsDisplay
                         {
                             Name = product.Name,
                             Quantity = product.Quantity,
                             Price = Math.Round(product.Price),
                             ProductType = ProductTypeEnum.Product
                         }
                         )?.ToList();
            productsDisplays.AddRange(query);


            var query1 = restaurantEntities.GetAllMenusWithPriceByCategory(category).ToList();
            foreach (var menu in query1)
            {
                productsDisplays.Add(new ProductsDisplay()
                {
                    Name = menu.Name,
                    Quantity = (int)(menu.Quantity ?? 0),
                    Price = Math.Round(MenuPriceAfterDiscount(menu.Price ?? 0)),
                    ProductType = ProductTypeEnum.Menu
                });
            }

            return productsDisplays;
        }

        public ProductsDisplay GetProductDetails(string productName)
        {
            try
            {
                var query = (from product in restaurantEntities.Product
                             where product.Name.Equals(productName)
                             select new ProductsDisplay
                             {
                                 Name = product.Name,
                                 Quantity = product.Quantity,
                                 Price = Math.Round(product.Price),
                                 CategoryName = product.Category.Name,
                                 Image1 = product.Image1,
                                 Image2 = product.Image2,
                                 Image3 = product.Image3,
                                 ProductType = ProductTypeEnum.Product
                             }).First();
                query.Allergens = restaurantEntities.GetAllergensByProduct(productName).ToList();

                return query;
            }
            catch
            {
                var query = (from menu in restaurantEntities.Menu
                             where menu.Name.Equals(productName)
                             select new ProductsDisplay
                             {
                                 Name = menu.Name,
                                 Quantity = (int)(from menu_product in restaurantEntities.Menu_Product
                                                  where menu.MenuID.Equals(menu_product.MenuID)
                                                  select menu_product.Quantity).Sum(),
                                 Price = Math.Round((from product in restaurantEntities.Product
                                          join menu_product in restaurantEntities.Menu_Product
                                          on product.ProductID equals menu_product.ProductID
                                          where menu.MenuID.Equals(menu_product.MenuID)
                                          select product.Price).Sum()),
                                 CategoryName = menu.Category.Name,
                                 Image1 = menu.Image1,
                                 Image2 = menu.Image2,
                                 Image3 = menu.Image3,
                                 ProductType = ProductTypeEnum.Menu
                             }).First();
                query.Price = Math.Round(MenuPriceAfterDiscount(query.Price));
                query.Allergens = restaurantEntities.GetAllergensByMenu(productName).ToList();
                query.Products = new List<Product>();
                var list = restaurantEntities.GetProductsByMenu(productName).ToList();
                foreach (var product in list)
                {
                    query.Products.Add(new Product()
                    {
                        ProductID = product.ProductID,
                        Name = product.Name,
                        Quantity = (int)product.Quantity,
                        Price = Math.Round(product.Price)
                    });
                }

                return query;
            }
        }

        public List<ProductsDisplay> GetProductsWithTotalQuantity(string categoryName)
        {
            List<ProductsDisplay> productsList = new List<ProductsDisplay>();
            var products = (from product in restaurantEntities.Product
                            where product.TotalQuantity <= Constants.MinTotalQuantity && product.Category.Name == categoryName
                            select product).ToList();

            foreach (var product in products)
            {
                productsList.Add(new ProductsDisplay
                {
                    Name = product.Name,
                    Quantity = (int)product.TotalQuantity
                });
            }

            return productsList;
        }
    }
}
