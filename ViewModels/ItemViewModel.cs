using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineSales.ViewModels
{
    public class ItemViewModel
    {
        public int ItemID { get; set; }

        public int CategoryID { get; set; }

        public int ItemBrandID { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        public string ItemSize { get; set; }

        [Display(Name = "Item Path")]
        public HttpPostedFileBase ImagePath { get; set; }

        [Display(Name = "Image Price")]
        public decimal ItemPrice { get; set; }

        public decimal Quantity { get; set; }

        public string Operator { get; set; }
    }
}