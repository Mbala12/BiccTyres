using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSales.Models;
using System.Web.Mvc;

namespace OnlineSales.Repositories
{
    public class CategoryRepository
    {
        private BiccTyresEntities biccTyre;

        public CategoryRepository()
        {
            biccTyre = new BiccTyresEntities();
        }

        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var objSelectItems = new List<SelectListItem>();
            objSelectItems = (from obj in biccTyre.Categories
                              select new SelectListItem
                              {
                                  Text = obj.CategoryName,
                                  Value = obj.CategoryID.ToString(),
                                  Selected = true
                              }).ToList();
            return objSelectItems;
        }
    }
}