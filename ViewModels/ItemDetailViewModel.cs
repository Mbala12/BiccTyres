using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineSales.ViewModels
{
    public class ItemDetailViewModel
    {
        public int ItemID { get; set; }

        public int CategoryID { get; set; }

        public string TransactionDate { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Item Image")]
        public string Image { get; set; }

        [Display(Name = "Image Price")]
        public decimal ItemPrice { get; set; }

        [Display(Name = "Item Size")]
        public string ItemSize { get; set; }

        [Display(Name = "Item Brand")]
        public string ItemBrand { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public decimal Quantity { get; set; }

        public bool InStock { get; set; }

        public string Operator { get; set; }
    }
}