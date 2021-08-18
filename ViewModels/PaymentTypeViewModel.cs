using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineSales.ViewModels
{
    public class PaymentTypeViewModel
    {
        public int PaymentTypeID { get; set; }

        [Display(Name ="Payment Type")]
        public string PaymentTypeName { get; set; }
    }
}