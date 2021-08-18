using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using OnlineSales.Models;
using OnlineSales.Repositories;
using OnlineSales.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace OnlineSales.Controllers
{
    [Authorize]
    [HandleError]
    public class ServiceController : Controller
    {
        private BiccTyresEntities biccTyre;
        public ServiceController()
        {
            biccTyre = new BiccTyresEntities();
        }
        // GET: Service
        public ActionResult Index()
        {
            ServiceRepository _serviceRepository = new ServiceRepository();
            ServiceCategoryRepository _serviceCategoryRepository = new ServiceCategoryRepository();

            var objMultitpleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                                    (_serviceRepository.GetAllServices(), _serviceCategoryRepository.GetAllServiceCategories());

            return View(objMultitpleModels);
        }

        [HttpPost]
        public JsonResult DoServices(ServiceViewModel objServiceViewModel)
        {
            ServiceRepository objServiceRepository = new ServiceRepository();
            objServiceRepository.DoAService(objServiceViewModel);

            return Json("New Service has been successfully made.", JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAlignmentDetails(int? i)
        {
            IEnumerable<ServiceViewModel> listOfAlignDetails = (
                from objAlign in biccTyre.Services
                where objAlign.ServCategoryID == 1
                select new ServiceViewModel()
                {
                    CustFullName = objAlign.CustFullName,
                    CustPhone = objAlign.CustPhone,
                    ODOMeter = objAlign.ODOMeter,
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    LicenseNo = objAlign.LicenseNo,
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    VIN = objAlign.VIN,
                    CreatedOn =objAlign.CreatedOn
                }).ToList().ToPagedList(i ?? 1, 15);

            return PartialView("_AlignmentDetailsPartial", listOfAlignDetails);
        }

        public PartialViewResult GetPunctureDetails(int? i)
        {
            IEnumerable<ServiceViewModel> listOfPunctDetails = (
                from objPunct in biccTyre.Services
                where objPunct.ServCategoryID == 2
                select new ServiceViewModel()
                {
                    CustFullName = objPunct.CustFullName,
                    CustPhone = objPunct.CustPhone,
                    OrderNumber = objPunct.OrderNumber,
                    Operator = objPunct.Operator,
                    UnitPrice = objPunct.UnitPrice,
                    Quantity = objPunct.Quantity,
                    SubTotal = objPunct.SubTotal,
                    CreatedOn = objPunct.CreatedOn
                }).ToList().ToPagedList(i ?? 1, 15);

            return PartialView("_PunctureDetailsPartial", listOfPunctDetails);
        }

        public PartialViewResult GetBalancingDetails(int? i)
        {
            IEnumerable<ServiceViewModel> listOfBalDetails = (
                from objPunct in biccTyre.Services
                where objPunct.ServCategoryID == 3
                select new ServiceViewModel()
                {
                    CustFullName = objPunct.CustFullName,
                    CustPhone = objPunct.CustPhone,
                    OrderNumber = objPunct.OrderNumber,
                    Operator = objPunct.Operator,
                    UnitPrice = objPunct.UnitPrice,
                    Quantity = objPunct.Quantity,
                    SubTotal = objPunct.SubTotal,
                    CreatedOn = objPunct.CreatedOn
                }).ToList().ToPagedList(i ?? 1, 5);

            return PartialView("_BalancingDetailsPartial", listOfBalDetails);
        }

        public PartialViewResult GetMagRepairDetails(int? i)
        {
            IEnumerable<ServiceViewModel> listOfMagDetails = (
                from objPunct in biccTyre.Services
                where objPunct.ServCategoryID == 4
                select new ServiceViewModel()
                {
                    CustFullName = objPunct.CustFullName,
                    CustPhone = objPunct.CustPhone,
                    OrderNumber = objPunct.OrderNumber,
                    Operator = objPunct.Operator,
                    UnitPrice = objPunct.UnitPrice,
                    Quantity = objPunct.Quantity,
                    SubTotal = objPunct.SubTotal,
                    CreatedOn = objPunct.CreatedOn
                }).ToList().ToPagedList(i ?? 1, 5);

            return PartialView("_MagRepairDetailsPartial", listOfMagDetails);
        }

        public PartialViewResult GetMechanicalDetails(int? i)
        {
            IEnumerable<ServiceViewModel> listOfMechDetails = (
                from objPunct in biccTyre.Services
                where objPunct.ServCategoryID == 5
                select new ServiceViewModel()
                {
                    CustFullName = objPunct.CustFullName,
                    CustPhone = objPunct.CustPhone,
                    OrderNumber = objPunct.OrderNumber,
                    Operator = objPunct.Operator,
                    UnitPrice = objPunct.UnitPrice,
                    Quantity = objPunct.Quantity,
                    SubTotal = objPunct.SubTotal,
                    CreatedOn = objPunct.CreatedOn
                }).ToList().ToPagedList(i ?? 1, 5);

            return PartialView("_MechanicalDetailsPartial", listOfMechDetails);
        }

        public ActionResult ExportAllAlignments()
        {
            IEnumerable<ServiceViewModel> listOfAligns = (
                from objAlign in biccTyre.Services
                where objAlign.ServCategoryID == 1
                select new ServiceViewModel()
                {
                    //CustFullName = objAlign.CustFullName,
                    //CustPhone = objAlign.CustPhone,
                    ODOMeter = objAlign.ODOMeter.ToUpper() + " Km/h",
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    //LicenseNo = objAlign.LicenseNo.ToUpper(),
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    VIN = objAlign.VIN.ToUpper(),
                    CreatedOn = objAlign.CreatedOn
                }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Alignments.rpt"));
            rd.SetDataSource(listOfAligns);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                if(DateTime.Now.Month < 10 && DateTime.Now.Day < 10)
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Alignments_" + DateTime.Now.Year + "0" + DateTime.Now.Month + "0" + DateTime.Now.Day + ".pdf");
                }
                else
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Alignments_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".pdf");
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ExportAllPunctures()
        {
            IEnumerable<ServiceViewModel> listOfPuncts = (
                from objAlign in biccTyre.Services
                where objAlign.ServCategoryID == 2
                select new ServiceViewModel()
                {
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    CreatedOn = objAlign.CreatedOn
                }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Punctures.rpt"));
            rd.SetDataSource(listOfPuncts);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                if (DateTime.Now.Month < 10 && DateTime.Now.Day < 10)
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Punctures_" + DateTime.Now.Year + "0" + DateTime.Now.Month + "0" + DateTime.Now.Day + ".pdf");
                }
                else
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Punctures_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".pdf");
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ExportAllBalancings()
        {
            IEnumerable<ServiceViewModel> listOfPuncts = (
                from objAlign in biccTyre.Services
                where objAlign.ServCategoryID == 3
                select new ServiceViewModel()
                {
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    CreatedOn = objAlign.CreatedOn
                }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Balancings.rpt"));
            rd.SetDataSource(listOfPuncts);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                if (DateTime.Now.Month < 10 && DateTime.Now.Day < 10)
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Balancings_" + DateTime.Now.Year + "0" + DateTime.Now.Month + "0" + DateTime.Now.Day + ".pdf");
                }
                else
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Balancings_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".pdf");
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ExportAllMagRepairs()
        {
            IEnumerable<ServiceViewModel> listOfPuncts = (
                from objAlign in biccTyre.Services
                where objAlign.ServCategoryID == 4
                select new ServiceViewModel()
                {
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    CreatedOn = objAlign.CreatedOn
                }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "MagRepairs.rpt"));
            rd.SetDataSource(listOfPuncts);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                if (DateTime.Now.Month < 10 && DateTime.Now.Day < 10)
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_MagRepairs_" + DateTime.Now.Year + "0" + DateTime.Now.Month + "0" + DateTime.Now.Day + ".pdf");
                }
                else
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_MagRepairs_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".pdf");
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ExportAllMechanicals()
        {
            IEnumerable<ServiceViewModel> listOfPuncts = (
                from objAlign in biccTyre.Services
                where objAlign.ServCategoryID == 5
                select new ServiceViewModel()
                {
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    CreatedOn = objAlign.CreatedOn
                }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Mechanicals.rpt"));
            rd.SetDataSource(listOfPuncts);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                if (DateTime.Now.Month < 10 && DateTime.Now.Day < 10)
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Mechanicals_" + DateTime.Now.Year + "0" + DateTime.Now.Month + "0" + DateTime.Now.Day + ".pdf");
                }
                else
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "AllDaily_Mechanicals_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".pdf");
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ExportAllServices()
        {
            IEnumerable<ServiceViewModel> listOfAllServices = (
                from objAlign in biccTyre.Services
                join objServCat in biccTyre.ServiceCategories on objAlign.ServCategoryID equals objServCat.ServCategoryID
                select new ServiceViewModel()
                {
                    ODOMeter = objAlign.ODOMeter.ToUpper() + " Km/h",
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    VIN = objAlign.VIN.ToUpper(),
                    ServiceCategoryName = objServCat.ServCategoryName,
                    CreatedOn = objAlign.CreatedOn
                }).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "AllServices.rpt"));
            rd.SetDataSource(listOfAllServices);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                if (DateTime.Now.Month < 10 && DateTime.Now.Day < 10)
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "All_Services_" + DateTime.Now.Year + "0" + DateTime.Now.Month + "0" + DateTime.Now.Day + ".pdf");
                }
                else
                {
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "All_Services_" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".pdf");
                }
            }
            catch
            {
                throw;
            }
        }
        public PartialViewResult GetAllServices(int? i)
        {
            IEnumerable<ServiceViewModel> listOfAllServices = (
                from objAlign in biccTyre.Services
                join objServCat in biccTyre.ServiceCategories on objAlign.ServCategoryID equals objServCat.ServCategoryID

                select new ServiceViewModel()
                {
                    CustFullName = objAlign.CustFullName,
                    CustPhone = objAlign.CustPhone,
                    ODOMeter = objAlign.ODOMeter,
                    OrderNumber = objAlign.OrderNumber,
                    Operator = objAlign.Operator,
                    LicenseNo = objAlign.LicenseNo,
                    UnitPrice = objAlign.UnitPrice,
                    Quantity = objAlign.Quantity,
                    SubTotal = objAlign.SubTotal,
                    VIN = objAlign.VIN,
                    ServiceCategoryName = objServCat.ServCategoryName,
                    CreatedOn = objAlign.CreatedOn
                }).ToList().ToPagedList(i ?? 1, 15);

            return PartialView("_AllServicesPartial", listOfAllServices);
        }
    }
}