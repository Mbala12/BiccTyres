using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSales.Models;
using OnlineSales.Repositories;
using OnlineSales.ViewModels;
using PagedList;


namespace OnlineSales.Controllers
{
    [Authorize]
    [HandleError]
    public class ItemController : Controller
    {
        private BiccTyresEntities biccTyre;
        public ItemController()
        {
            biccTyre = new BiccTyresEntities();
        }

        // GET: Item
        public ActionResult Index()
        {
            CategoryRepository _categoryRepository = new CategoryRepository();
            ItemRepository _itemRepository = new ItemRepository();
            ItemBrandRepository _itemBrandRepository = new ItemBrandRepository();

            var objMultitpleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                                    (_categoryRepository.GetAllCategories(), _itemRepository.GetAllItems(),  _itemBrandRepository.GetAllItemBrands());

            return View(objMultitpleModels);
        }

        [HttpPost]
        public ActionResult Index(ItemViewModel objItemViewModel)
        {
            string Message = "";
            string ActualImageName = string.Empty;
            string ImageuniqueName = string.Empty;
            if (objItemViewModel.ItemID == 0)
            {
                ImageuniqueName = Guid.NewGuid().ToString();
                ActualImageName = ImageuniqueName + Path.GetExtension(objItemViewModel.ImagePath.FileName);
                objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + ActualImageName));
                Item objtItem = new Item()
                {
                    ItemID = objItemViewModel.ItemID,
                    CategoryID = objItemViewModel.CategoryID,
                    ItemBrandID = objItemViewModel.ItemBrandID,
                    ItemSize = objItemViewModel.ItemSize.ToUpper(),
                    ItemName = objItemViewModel.ItemName.ToUpper(),
                    ItemPrice = objItemViewModel.ItemPrice,
                    ImagePath = ActualImageName,
                    Operator = objItemViewModel.Operator.ToUpper(),
                    InStock = true
                };
                biccTyre.Items.Add(objtItem);
                biccTyre.SaveChanges();
                Transaction objTransaction = new Transaction();
                objTransaction.ItemID = objtItem.ItemID;
                objTransaction.TransactionTypeID = 1;
                objTransaction.TransactionDate = DateTime.Now.ToShortDateString();
                objTransaction.Quantity = objItemViewModel.Quantity;
                objTransaction.Operator = objItemViewModel.Operator.ToUpper();
                biccTyre.Transactions.Add(objTransaction);
                biccTyre.SaveChanges();
                Message = "New Item has been successfully Added.";
            }
            else
            {
                Item objItem = biccTyre.Items.Single(model => model.ItemID == objItemViewModel.ItemID);
                if (objItemViewModel.ImagePath != null)
                {
                    ImageuniqueName = Guid.NewGuid().ToString();
                    ActualImageName = ImageuniqueName + Path.GetExtension(objItemViewModel.ImagePath.FileName);
                    objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + ActualImageName));
                    objItem.ImagePath = ActualImageName;
                }
                objItem.ItemName = objItemViewModel.ItemName.ToUpper();
                objItem.ItemPrice = objItemViewModel.ItemPrice;
                objItem.CategoryID = objItemViewModel.CategoryID;
                objItem.ItemBrandID = objItemViewModel.ItemBrandID;
                objItem.ItemSize = objItemViewModel.ItemSize.ToUpper();
                objItem.Operator = objItemViewModel.Operator.ToUpper();
                //objItem.InStock = true;
                Transaction objTrans = biccTyre.Transactions.Single(model => model.ItemID == objItem.ItemID);
                objTrans.ItemID = objItem.ItemID;
                objTrans.TransactionTypeID = 1;
                objTrans.TransactionDate = DateTime.Now.ToString();
                objTrans.Quantity = objItemViewModel.Quantity;
                objTrans.Operator = objItemViewModel.Operator.ToUpper();
                biccTyre.Transactions.Add(objTrans);
                biccTyre.SaveChanges();
                Message = "This Item has been successfully Updated.";
                //biccTyre.SaveChanges();
            }
            //biccTyre.SaveChanges();
            return Json(new { message = Message, success = true }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAllItemsInStock(int? i)
        {
            IEnumerable<ItemDetailViewModel> listOfAllItems = (
                from objItem in biccTyre.Items
                join objCat in biccTyre.Categories on objItem.CategoryID equals objCat.CategoryID
                join objBrand in biccTyre.ItemBrands on objItem.ItemBrandID equals objBrand.ItemBrandID
                join objTrans in biccTyre.Transactions on objItem.ItemID equals objTrans.ItemID
                where objItem.InStock == true && objTrans.TransactionTypeID == 1
                select new ItemDetailViewModel()
                {
                    ItemID = objItem.ItemID,
                    Operator = objItem.Operator,
                    TransactionDate = objTrans.TransactionDate,
                    CategoryName = objCat.CategoryName,
                    ItemSize = objItem.ItemSize,
                    ItemBrand = objBrand.ItemBrandName,
                    ItemName = objItem.ItemName,
                    ItemPrice = objItem.ItemPrice,
                    Image = objItem.ImagePath
                }).ToList().ToPagedList(i ?? 1, 5);
            
            return PartialView("_ItemDetailInStockPartial", listOfAllItems);
        }

        public PartialViewResult GetAllItemsHaveSold(int? i)
        {
            IEnumerable<ItemDetailViewModel> listOfAllItems = (
                from objItem in biccTyre.Items
                join objCat in biccTyre.Categories on objItem.CategoryID equals objCat.CategoryID
                join objBrand in biccTyre.ItemBrands on objItem.ItemBrandID equals objBrand.ItemBrandID
                join objTrans in biccTyre.Transactions on objItem.ItemID equals objTrans.ItemID
                where objItem.InStock == true && objTrans.TransactionTypeID == 2
                select new ItemDetailViewModel()
                {
                    ItemID = objItem.ItemID,
                    Quantity = (-1) * objTrans.Quantity,
                    TransactionDate = objTrans.TransactionDate,
                    CategoryName = objCat.CategoryName,
                    ItemSize = objItem.ItemSize,
                    ItemBrand = objBrand.ItemBrandName,
                    ItemName = objItem.ItemName,
                    ItemPrice = objItem.ItemPrice,
                    Image = objItem.ImagePath
                }).ToList().ToPagedList(i ?? 1, 5);

            return PartialView("_ItemDetailHaveSoldPartial", listOfAllItems);
        }

        public PartialViewResult ShowQuantityAvailable(int? i)
        {
            IEnumerable<ItemDetailViewModel> listOfAvailQty = (
               from objItem in biccTyre.Items
               join objCat in biccTyre.Categories on objItem.CategoryID equals objCat.CategoryID
               join objBrand in biccTyre.ItemBrands on objItem.ItemBrandID equals objBrand.ItemBrandID
               join objTrans in biccTyre.Transactions on objItem.ItemID equals objTrans.ItemID into grp
               select new ItemDetailViewModel()
               {
                   ItemSize = objItem.ItemSize,
                   ItemName = objItem.ItemName,
                   ItemBrand = objBrand.ItemBrandName,
                   CategoryName = objCat.CategoryName,
                   Quantity = grp.Sum(x => x.Quantity)
               }).ToList().ToPagedList(i ?? 1, 5);
            return PartialView("_ShowQuantityAvailablePartial", listOfAvailQty);
        }

        [HttpGet]
        public JsonResult EditItemDetails(int itemID)
        {
            var result = biccTyre.Items.Single(model => model.ItemID == itemID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RemoveItemDetails(int itemID)
        {
            var objItem = biccTyre.Items.Single(model => model.ItemID == itemID);
            objItem.InStock = false;
            biccTyre.SaveChanges();
            return Json(new { message = "Item has been Successfully Deleted.", success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}