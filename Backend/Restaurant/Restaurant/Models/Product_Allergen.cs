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
    
    public partial class Product_Allergen
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int AllergenID { get; set; }
    
        public virtual Allergen Allergen { get; set; }
        public virtual Product Product { get; set; }
    }
}
