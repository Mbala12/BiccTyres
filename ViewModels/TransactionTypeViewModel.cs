using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineSales.ViewModels
{
    public class TransactionTypeViewModel
    {
        public int TransactionTypeID { get; set; }

        [Display(Name="Transaction Name")]
        public string TransactionName { get; set; }
    }
}