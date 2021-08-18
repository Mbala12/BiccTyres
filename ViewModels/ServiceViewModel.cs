using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSales.ViewModels
{
    public class ServiceViewModel
    {
        public int ServID { get; set; }

        public string CustFullName { get; set; }

        public string CustPhone { get; set; }

        public string LicenseNo { get; set; }

        public string ODOMeter { get; set; }

        public string OrderNumber { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal SubTotal { get; set; }

        public string VIN { get; set; }

        public string Operator { get; set; }

        public string CreatedOn { get; set; }

        public int ServCategoryID { get; set; }

        public string ServiceCategoryName { get; set; }
    }
}