//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Restaurant.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu_Product
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int ProductID { get; set; }
        public double Quantity { get; set; }
    
        public virtual Menu Menu { get; set; }
        public virtual Product Product { get; set; }
    }
}
