using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineSales.ViewModels
{
    public class TransactionViewModel
    {
        public int TransactionID { get; set; }

        public int ItemID { get; set; }

        public string TransactionTypeID { get; set; }

        public decimal Quantity { get; set; }

        public string TransactionDate { get; set; }

        public string Operation { get; set; }
    }
}