using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSales.Models;
using System.Web.Mvc;

namespace OnlineSales.Repositories
{
    public class PaymentTypeRepository
    {
        private BiccTyresEntities biccTyre;
        public PaymentTypeRepository()
        {
            biccTyre = new BiccTyresEntities();
        }

        public IEnumerable<SelectListItem> GetAllPaymentTypes()
        {
            var objSelectItems = new List<SelectListItem>();
            objSelectItems = (from obj in biccTyre.PaymentTypes
                              select new SelectListItem
                              {
                                  Text = obj.PaymentTypeName,
                                  Value = obj.PaymentTypeID.ToString(),
                                  Selected = true
                              }).ToList();
            return objSelectItems;
        }
    }
}