using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class PaymentViewModel
    {
        public int? VoucherID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int? PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public string PromoCodes { get; set; }
        public int AvailableQuantity { get; set; }
        public int Quantity { get; set; }
        public decimal? Amount { get; set; }
        public string VoucherTitle { get; set; }
        public int? Discount { get; set; }
      
    }

    public class ReturnPaymentViewModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public string secretkey { get; set; }
        public List<PurchaseViewModel> list { get; set; }
    }

    public class PurchaseViewModel
    {
        public int ID { get; set; }
        public int? VoucherID { get; set; }
        public string VoucherName { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime? Accesstime { get; set; }
        public int? Quantity { get; set; }
        public string PromoCodes { get; set; }
        public int? PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal? Amount { get; set; }
        public int? Discount { get; set; }
        public decimal? TotalAmount { get; set; }
    }


    public class ReturnViewModel
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class ReturnVerifyPromocodesViewModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public string promocodes { get; set; }
    }

    public class PurchaseHistoryViewModel
    {
        public int? VoucherID { get; set; }
        public string VoucherTitle { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public Nullable<int> Discount { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public string PromoCodes { get; set; }
        public int? Quantity { get; set; }
        public string Type { get; set; }
      
    }

    public class VoucherViewModel
    {
        public int? VoucherID { get; set; }
        public string Title { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Accesstime { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public Nullable<int> Discount { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? Quantity { get; set; }
        public int? UsedQuantity { get; set; }
        public string Type { get; set; }
        public bool? IsActive { get; set; }
    }

    public class VerifyViewModel
    {
        public string PromoCodes { get; set; }
    }


    public class CheckOutbyEVoucherViewModel
    {
        public string Products { get; set; }
        public int? VoucherID { get; set; }
        public string PromoCodes { get; set; }


    }



    public class Item
    {
        public string Id { get; set; }
    }

    public class PaymentIntentCreateRequest
    {
        public Item[] Items { get; set; }
    }




}
