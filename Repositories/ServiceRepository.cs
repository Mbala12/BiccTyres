using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSales.Models;
using System.Web.Mvc;
using OnlineSales.ViewModels;

namespace OnlineSales.Repositories
{
    public class ServiceRepository
    {
        private BiccTyresEntities biccTyre;
        public ServiceRepository()
        {
            biccTyre = new BiccTyresEntities();
        }

        public IEnumerable<SelectListItem> GetAllServices()
        {
            var objSelectItems = new List<SelectListItem>();
            objSelectItems = (from obj in biccTyre.Services
                              select new SelectListItem
                              {
                                  Text = obj.OrderNumber,
                                  Value = obj.ServID.ToString(),
                                  Selected = true
                              }).ToList();
            return objSelectItems;
        }

        public bool DoAService(ServiceViewModel objServiceViewModel)
        {
            Service objServ = new Service();
            if(objServ.LicenseNo == null && objServ.ODOMeter == null && objServ.VIN == null)
            {
                objServ.CreatedOn = DateTime.Now.ToShortDateString();
                objServ.Operator = objServiceViewModel.Operator;
                objServ.OrderNumber = String.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
                objServ.CustFullName = objServiceViewModel.CustFullName;
                objServ.CustPhone = objServiceViewModel.CustPhone;
                objServ.LicenseNo = objServiceViewModel.LicenseNo;
                objServ.ODOMeter = objServiceViewModel.ODOMeter;
                objServ.UnitPrice = objServiceViewModel.UnitPrice;
                objServ.Quantity = objServiceViewModel.Quantity;
                objServ.SubTotal = objServiceViewModel.SubTotal;
                objServ.VIN = objServiceViewModel.VIN;
                objServ.ServCategoryID = objServiceViewModel.ServCategoryID;
                objServ.ServID = objServiceViewModel.ServID;
                biccTyre.Services.Add(objServ);
                biccTyre.SaveChanges();
            }
            else
            {
                objServ.CreatedOn = DateTime.Now.ToShortDateString();
                objServ.Operator = objServiceViewModel.Operator;
                objServ.OrderNumber = String.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
                objServ.CustFullName = objServiceViewModel.CustFullName;
                objServ.CustPhone = objServiceViewModel.CustPhone;
                objServ.LicenseNo = objServiceViewModel.LicenseNo;
                objServ.ODOMeter = objServiceViewModel.ODOMeter;
                objServ.UnitPrice = objServiceViewModel.UnitPrice;
                objServ.Quantity = objServiceViewModel.Quantity;
                objServ.SubTotal = objServiceViewModel.SubTotal;
                objServ.VIN = objServiceViewModel.VIN;
                objServ.ServCategoryID = objServiceViewModel.ServCategoryID;
                objServ.ServID = objServiceViewModel.ServID;
                biccTyre.Services.Add(objServ);
                biccTyre.SaveChanges();
            }

            return true;
        }
    }
}