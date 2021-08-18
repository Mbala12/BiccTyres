using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineSales.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [Display(Name ="Category Code")]
        public string CategoryCode { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
    }
}