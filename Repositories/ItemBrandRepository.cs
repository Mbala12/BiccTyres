using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSales.Models;
using System.Web.Mvc;

namespace OnlineSales.Repositories
{
    public class ItemBrandRepository
    {
        private BiccTyresEntities biccTyre;
        public ItemBrandRepository()
        {
            biccTyre = new BiccTyresEntities();
        }

        public IEnumerable<SelectListItem> GetAllItemBrands()
        {
            var objSelectItems = new List<SelectListItem>();
            objSelectItems = (from obj in biccTyre.ItemBrands
                                  select new SelectListItem
                                  {
                                      Text = obj.ItemBrandName,
                                      Value = obj.ItemBrandID.ToString(),
                                      Selected = true
                                  }).ToList();
            return objSelectItems;
        }
    }
}