//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineSales.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemSize { get; set; }
        public decimal ItemPrice { get; set; }
        public string ImagePath { get; set; }
        public int CategoryID { get; set; }
        public int ItemBrandID { get; set; }
        public bool InStock { get; set; }
        public string Operator { get; set; }
    }
}
