using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineSales.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int ItemID { get; set; }

        [Display(Name ="Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        public string Operator { get; set; }
    }
}