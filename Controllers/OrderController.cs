using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSales.Models;
using OnlineSales.Repositories;
using OnlineSales.ViewModels;

namespace OnlineSales.Controllers
{
    [Authorize]
    [HandleError]
    public class OrderController : Controller
    {
        private BiccTyresEntities biccTyre;
        public OrderController()
        {
            biccTyre = new BiccTyresEntities();
        }
        // GET: Order
        public ActionResult Index()
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            ItemRepository itemRepository = new ItemRepository();
            PaymentTypeRepository paymentTypeRepository = new PaymentTypeRepository();
           
            var objMultipleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                                    (categoryRepository.GetAllCategories(), itemRepository.GetAllItems(), paymentTypeRepository.GetAllPaymentTypes());
            return View(objMultipleModels);
        }

        [HttpGet]
        public JsonResult GetSelectedItems(int categoryID)
        {
            //onFood.Configuration.ProxyCreationEnabled = false;
            List<Item> ListItems = biccTyre.Items.Where(x => x.CategoryID == categoryID).ToList();
            return Json(ListItems, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetItemQuantity(int itemID)
        //{
        //    var result = (from obj in biccTyre.Items
        //                  join objB in biccTyre.ItemBrands on obj.ItemBrandID equals objB.ItemBrandID
        //                  join objT in biccTyre.Transactions on obj.ItemID equals objT.ItemID
        //                  where obj.ItemID == itemID && objT.ItemID == itemID && objT.TransactionTypeID == 1  
        //                  group objT by obj into g
        //                  select new
        //                  {
        //                      Quantity = g.Sum(x => x.Quantity)
        //                  }
        //                  ).FirstOrDefault();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetItemUnitPrice(int itemID)
        {
            var result = (from obj in biccTyre.Items
                          join objB in biccTyre.ItemBrands on obj.ItemBrandID equals objB.ItemBrandID
                          join objT in biccTyre.Transactions on obj.ItemID equals objT.ItemID //into g
                          where obj.ItemID == itemID && objT.ItemID == itemID && objT.TransactionTypeID == 1

                          select new
                          {
                              ItemBrand = objB.ItemBrandName,
                              ItemPrice = obj.ItemPrice,
                              ItemImage = obj.ImagePath,
                              ItemName = obj.ItemName,
                              Qty = objT.Quantity//g.Sum(x => x.Quantity)
                          }
                          ).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Index(OrderViewModel objOrderViewModel)
        {
            OrderRepository objOrderRepository = new OrderRepository();
            objOrderRepository.AddOrder(objOrderViewModel);

            return Json("Your Operation has been successfully made.", JsonRequestBehavior.AllowGet);
        }
    }
}