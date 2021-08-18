using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSales.Models;
using System.Web.Mvc;

namespace OnlineSales.Repositories
{
    public class ServiceCategoryRepository
    {
        private BiccTyresEntities biccTyre;
        public ServiceCategoryRepository()
        {
            biccTyre = new BiccTyresEntities();
        }

        public IEnumerable<SelectListItem> GetAllServiceCategories()
        {
            var objSelectItems = new List<SelectListItem>();
            objSelectItems = (from obj in biccTyre.ServiceCategories
                              select new SelectListItem
                              {
                                  Text = obj.ServCategoryName,
                                  Value = obj.ServCategoryID.ToString(),
                                  Selected = true
                              }).ToList();
            return objSelectItems;
        }
    }
}