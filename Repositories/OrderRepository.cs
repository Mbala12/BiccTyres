using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSales.Models;
using OnlineSales.ViewModels;

namespace OnlineSales.Repositories
{
    public class OrderRepository
    {
        private BiccTyresEntities biccTyre;
        public OrderRepository()
        {
            biccTyre = new BiccTyresEntities();
        }

        public bool AddOrder(OrderViewModel objOrderViewModel)
        {
            Order objOrder = new Order();
            objOrder.CustomerName = objOrderViewModel.CustomerName;
            objOrder.CustomerPhone = objOrderViewModel.CustomerPhone;
            objOrder.CustomerAddress = objOrderViewModel.CustomerEmail;
            objOrder.DailyDate = DateTime.Now.ToShortDateString();
            objOrder.FinalTotal = objOrderViewModel.FinalTotal;
            objOrder.OrderDate = DateTime.Now;
            objOrder.OrderNumber = String.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
            objOrder.PaymentTypeID = objOrderViewModel.PaymentTypeID;
            objOrder.Operator = objOrderViewModel.Operator;
            biccTyre.Orders.Add(objOrder);
            biccTyre.SaveChanges();

            int orderID = objOrder.OrderID;
            foreach (var item in objOrderViewModel.ListOfOrderDetailViewModel)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.OrderID = orderID;
                objOrderDetail.ItemID = item.ItemID;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.Total = item.Total;
                objOrderDetail.Discount = item.Discount;
                objOrderDetail.Operator = item.Operator;
                biccTyre.OrderDetails.Add(objOrderDetail);
                biccTyre.SaveChanges();

                Transaction objTransaction = new Transaction();
                objTransaction.TransactionDate = DateTime.Now.ToShortDateString();
                objTransaction.TransactionTypeID = 2;
                objTransaction.Quantity = (-1) * item.Quantity;
                objTransaction.ItemID = item.ItemID;
                objTransaction.Operator = item.Operator;
                biccTyre.Transactions.Add(objTransaction);
                biccTyre.SaveChanges();
            }

            return true;
        }
    }
}