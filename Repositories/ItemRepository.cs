using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSales.Models;

namespace OnlineSales.Repositories
{
    public class ItemRepository
    {
        private BiccTyresEntities biccTyre;
        public ItemRepository()
        {
            biccTyre = new BiccTyresEntities();
        }

        public IEnumerable<SelectListItem> GetAllItems()
        {
            var objSelectItems = new List<SelectListItem>();
            objSelectItems = (from obj in biccTyre.Items
                              //join objBrand in biccTyre.ItemBrands on obj.ItemBrandID equals objBrand.ItemBrandID
                              //join objCat in biccTyre.Categories on obj.CategoryID equals objCat.CategoryID
                              //where obj.CategoryID == objCat.CategoryID && obj.ItemBrandID == objBrand.ItemBrandID
                              select new SelectListItem
                              {
                                  Text = obj.ItemSize,
                                  Value = obj.ItemID.ToString(),
                                  Selected = true
                              }).ToList();
            return objSelectItems;
        }
    }
}