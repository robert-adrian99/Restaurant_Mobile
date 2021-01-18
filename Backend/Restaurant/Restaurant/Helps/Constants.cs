using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Helps
{
    class Constants
    {
        static public double MenuDiscount
        {
            get
            {
                return 20;
                //return double.Parse(ConfigurationManager.AppSettings.Get("menuDiscount"));
            }
        }

        static public double DeliveryCost
        {
            get
            {
                return 15;
                //return double.Parse(ConfigurationManager.AppSettings.Get("deliveryCost"));
            }
        }

        static public double OrderDiscount
        {
            get
            {
                return 10;
                //return double.Parse(ConfigurationManager.AppSettings.Get("orderDiscount"));
            }
        }

        static public int DiscountTime
        {
            get
            {
                return 7;
                //return int.Parse(ConfigurationManager.AppSettings.Get("discountTime"));
            }
        }

        static public double OrderPriceGreaterThanForDelivery
        {
            get
            {
                return 50;
                //return double.Parse(ConfigurationManager.AppSettings.Get("orderPriceGreaterThanForDelivery"));
            }
        }

        static public double NumberMinOrders
        {
            get
            {
                return 5;
                //return double.Parse(ConfigurationManager.AppSettings.Get("numberMinOrders"));
            }
        }

        static public double OrderPriceGreaterThanForDiscount
        {
            get
            {
                return 70;
                //return double.Parse(ConfigurationManager.AppSettings.Get("orderPriceGreaterThanForDiscount"));
            }
        }

        static public int DeliveryTime 
        { 
            get
            {
                return 45;
                //return int.Parse(ConfigurationManager.AppSettings.Get("deliveryTime"));
            }
        }

        static public int MinTotalQuantity
        {
            get
            {
                return 1000;
                //return int.Parse(ConfigurationManager.AppSettings.Get("minTotalQuantity"));
            }
        }
    }
}
