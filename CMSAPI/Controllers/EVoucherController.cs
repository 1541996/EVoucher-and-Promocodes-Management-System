using CMSWeb.Helper;
using Core.Extensions;
using Data.Models;
using Data.ViewModels;
using Infra.Services;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
//using CMSAPI.Helper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EVoucherController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

       
        public readonly ApplicationDbContext _db;
        UnitOfWork uow;
        public EVoucherController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            this.uow = new UnitOfWork(_db);
            _webHostEnvironment = webHostEnvironment;

        }
       
        // GET: api/<EVoucherController>
        [Route("GetVoucherList")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(int Page = 1, int Pagesize = 10)
        {

            var objs = _db.vouchers.Where(a => a.IsDeleted != true).OrderByDescending(a => a.Accesstime);

            var totalCount = objs.Count();

            var results = objs.Skip(Pagesize * (Page - 1)).Take(Pagesize).AsQueryable();


            var resultdata = (from r in
                              results
                              select new VoucherViewModel
                              {
                                  VoucherID = r.ID,
                                  Title = r.Title,
                                  Amount = r.Amount,
                                  Accesstime = r.Accesstime,
                                  Description = r.Description,
                                  ExpiredDate = r.ExpiredDate,
                                  Photo = r.Photo,                                
                                  Quantity = r.Quantity,
                                  UsedQuantity = r.UsedQuantity,
                                  Type = r.Type,
                                  IsActive = r.IsActive,
                              }).ToList();

            PagedListServer<VoucherViewModel> model = new PagedListServer<VoucherViewModel>(resultdata, totalCount, Pagesize);
            return Ok(model);

       
        }

        // GET api/<EVoucherController>/5
        [HttpGet]
        [Route("GetVoucherDetail")]
        public IActionResult Get(int ID)
        {
            var model =  _db.vouchers.Where(a => a.IsDeleted != true && a.ID == ID).FirstOrDefault();
            return Ok(model);
        }


        [HttpGet]
        [Route("GetCheckOutData")]
        public IActionResult GetCheckOutData(int ID)
        {
            var voucher = _db.vouchers.Where(a => a.ID == ID && a.IsDeleted != true && a.IsActive == true).FirstOrDefault();
            PaymentViewModel pvm = new PaymentViewModel()
            {
                VoucherTitle = voucher.Title,
                VoucherID = voucher.ID,
                Amount = voucher.Amount,
                AvailableQuantity = voucher.Quantity ?? 0,
                Quantity = 1
            };
            return Ok(pvm);
        }

        [HttpGet]
        [Route("SetActive")]
        public IActionResult SetActive(int ID)
        {
            var model = _db.vouchers.Where(a => a.IsDeleted != true && a.ID == ID).FirstOrDefault();
            if(model.IsActive == false)
            {
                model.IsActive = true;
            }
            else
            {
                model.IsActive = false;
            }
            var updatedentity = uow.voucherRepo.UpdateWithObj(model);
            return Ok(updatedentity);
        }

        // POST api/<EVoucherController>
        [HttpPost]
        [Route("SaveVoucher")]
        public IActionResult Post(tbvoucher data)
        {
            // create file path
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string path = "";
            path = Path.Combine(contentRootPath, "Voucher");



            tbvoucher updatedEntity = null;
            var photoresult = "";

            FileUploadViewModel FVM = new FileUploadViewModel()
            {
                filepath = path,
                photo = data.Photo
            };

            if (data.Photo != null)
            {
                photoresult = FileUploadHelper.upload(FVM);           
            }
            else
            {
                photoresult = null;
            }





            if (data.ID > 0)
            {
                if (data.Photo == null)
                {
                    tbvoucher existed = _db.vouchers.AsNoTracking().FirstOrDefault(a => a.IsDeleted != true && a.ID == data.ID);
                    data.Photo = existed.Photo;
                }
                else
                {
                    data.Photo = photoresult;
                }
                data.Accesstime = MyExtension.getLocalTime(DateTime.UtcNow);
                updatedEntity = uow.voucherRepo.UpdateWithObj(data);
            }
            else
            {
                if (data.Photo != null)
                {
                    data.Photo = photoresult;
                }                   
                data.IsActive = true;
                data.UsedQuantity = 0;
                data.IsDeleted = false;
                data.Accesstime = MyExtension.getLocalTime(DateTime.UtcNow);
                updatedEntity = uow.voucherRepo.InsertReturn(data);
            }
           
           

            return Ok(updatedEntity);
        }

        // DELETE api/<EVoucherController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("GetPaymentMethodList")]
        [HttpGet]
        public IActionResult GetAllPaymentMethod()
        {
            var model = _db.paymentmethods.Where(a => a.IsDeleted != true).ToList();
            return Ok(model);
           
        }


        [Route("MakePayment")]
        [HttpPost]
        public IActionResult MakePayment(PaymentViewModel model)
        {
            // quantity check
            // using var transaction = _db.Database.BeginTransaction();
            ReturnPaymentViewModel rvm = new ReturnPaymentViewModel();
            try
            {
                var voucher = _db.vouchers.FirstOrDefault(a => a.IsDeleted != true && a.ID == model.VoucherID);
                bool IsQtyAvailable = false;
                int? availableqty = 0;
                if (voucher != null)
                {
                    availableqty = voucher.Quantity;
                    if (model.Quantity <= availableqty)
                    {
                        IsQtyAvailable = true;
                    }
                }

               
                if (IsQtyAvailable)
                {
                    try
                    {
                        var today = MyExtension.getLocalTime(DateTime.UtcNow);

                        string customerid = Guid.NewGuid().ToString();

                        tbcustomer customer = new tbcustomer()
                        {
                            //  ID = customerid,
                            Name = model.CustomerName,
                            PhoneNo = model.CustomerPhone,
                            Accesstime = today,
                            IsDeleted = false,

                        };

                        tbcustomer updatedCustomer = uow.customerRepo.InsertReturn(customer);


                        List<PurchaseViewModel> plist = new List<PurchaseViewModel>();
                        for (var i = 0; i < model.Quantity; i++)
                        {
                            tbpurchase purchase = new tbpurchase()
                            {
                                CustomerID = updatedCustomer.ID,
                                VoucherID = model.VoucherID,
                                Accesstime = today,
                                Quantity = 1,
                                PaymentMethodID = model.PaymentMethodID,
                                PromoCodes = getPromoCodes(model.CustomerPhone),
                                Amount = model.Amount,
                                Discount = model.Discount,
                                TotalAmount = model.Discount != null ? model.Amount - ((model.Amount * model.Discount) / 100) : model.Amount
                               
                            };
                            tbpurchase updatedPurchase = uow.purchaseRepo.InsertReturn(purchase);
                            PurchaseViewModel pvm = new PurchaseViewModel()
                            {
                                CustomerID = updatedCustomer.ID,
                                CustomerName = updatedCustomer.Name,
                                VoucherID = updatedPurchase.VoucherID,
                                VoucherName = voucher.Title,
                                Accesstime = updatedPurchase.Accesstime,
                                Quantity = 1,
                                PaymentMethodID = updatedPurchase.PaymentMethodID,
                                PaymentMethodName = model.PaymentMethodName,
                                PromoCodes = updatedPurchase.PromoCodes,
                                Amount = updatedPurchase.Amount,
                                Discount = updatedPurchase.Discount,
                                TotalAmount = updatedPurchase.TotalAmount,

                            };
                            plist.Add(pvm);
                        }

                        if (voucher.UsedQuantity >= 0)
                        {
                            voucher.UsedQuantity += model.Quantity;
                        }

                        if(voucher.Quantity >= 0)
                        {
                            voucher.Quantity -= model.Quantity;
                        }

                        voucher = uow.voucherRepo.UpdateWithObj(voucher);

                        if (voucher != null)
                        {
                            rvm = new ReturnPaymentViewModel()
                            {
                                status = "Success",
                                message = "Payment Successful",
                                list = plist
                            };
                        }
                        else
                        {
                            rvm = new ReturnPaymentViewModel()
                            {
                                status = "Fail",
                                message = "Payment Fail",

                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        rvm = new ReturnPaymentViewModel()
                        {
                            status = "Fail",
                            message = ex.Message,

                        };
                    }
                }
                else
                {
                    rvm = new ReturnPaymentViewModel()
                    {
                        status = "Quantity is not available.",
                        message = $"{availableqty} available",

                    };
                }


            }
            catch (DbUpdateConcurrencyException ex)
            {
                rvm = new ReturnPaymentViewModel()
                {
                    status = "Concurrent Update",
                    message = ex.Message,

                };
            }
            
          

            return Ok(rvm);
        }


        [Route("GetPurchaseHistoryList")]
        [HttpGet]
        public IActionResult PurchaseHistory(string Status = null, int Page = 1, int Pagesize = 10)
        {
            Expression<Func<tbpurchase, bool>> statusfilter = null;
            if (Status == "Used")
            {
                statusfilter = a => a.IsUsed == true;
            }
            else
            {
                statusfilter = a => a.IsUsed != true;
            }

            var objs = (from purchase in _db.purchases.Where(a => a.IsDeleted != true).Where(statusfilter)
                        join voucher in _db.vouchers.Where(a => a.IsDeleted != true)
                         on purchase.VoucherID equals voucher.ID
                        join customer in _db.customers.Where(a => a.IsDeleted != true)
                        on purchase.CustomerID equals customer.ID
                        join payment in _db.paymentmethods.Where(a => a.IsDeleted != true)
                        on purchase.PaymentMethodID equals payment.ID
                        select new { voucher, customer, purchase, payment }).OrderBy(a => a.voucher.ID);

            var totalCount = objs.Count();



            var results = objs.OrderBy(a => a.voucher.ID).Skip(Pagesize * (Page - 1)).Take(Pagesize).AsQueryable();



            var resultdata = (from r in
                              results
                              select new PurchaseHistoryViewModel
                              {
                                  VoucherID = r.voucher.ID,
                                  VoucherTitle = r.voucher.Title,
                                  CustomerName = r.customer.Name,
                                  CustomerPhone = r.customer.PhoneNo,
                                  Amount = r.purchase.Amount,
                                  TotalAmount = r.purchase.TotalAmount,
                                  Quantity = r.purchase.Quantity,
                                  Discount = r.purchase.Discount,
                                  Photo = r.voucher.Photo,
                                  Description = r.voucher.Description,
                                  ExpiredDate = r.voucher.ExpiredDate,
                                  PaymentMethodID = r.payment.ID,
                                  PaymentMethodName = r.payment.Name,
                                  PromoCodes = r.purchase.PromoCodes,
                                  Type = r.voucher.Type,

                              }).ToList();

            PagedListServer<PurchaseHistoryViewModel> model = new PagedListServer<PurchaseHistoryViewModel>(resultdata, totalCount, Pagesize);
            return Ok(model);
        }



        // need to test with credit card
        [Route("MakeActualPayment")]
        [HttpPost]
        public IActionResult MakeActualPayment(PaymentViewModel model)
        {
            decimal? TotalAmount = 0;
            var voucher = _db.vouchers.FirstOrDefault(a => a.IsDeleted != true && a.ID == model.VoucherID);
            bool IsQtyAvailable = false;
            int? availableqty = 0;
            if (voucher != null)
            {
                availableqty = voucher.Quantity;
                if (model.Quantity <= availableqty)
                {
                    IsQtyAvailable = true;
                }
            }

            ReturnPaymentViewModel rvm = new ReturnPaymentViewModel();
            if (IsQtyAvailable)
            {
                try
                {
                    var today = MyExtension.getLocalTime(DateTime.UtcNow);

                    string customerid = Guid.NewGuid().ToString();

                    tbcustomer customer = new tbcustomer()
                    {
                        //  ID = customerid,
                        Name = model.CustomerName,
                        PhoneNo = model.CustomerPhone,
                        Accesstime = today,
                        IsDeleted = false,

                    };

                    tbcustomer updatedCustomer = uow.customerRepo.InsertReturn(customer);


                    List<PurchaseViewModel> plist = new List<PurchaseViewModel>();
                    for (var i = 0; i < model.Quantity; i++)
                    {
                        tbpurchase purchase = new tbpurchase()
                        {
                            CustomerID = updatedCustomer.ID,
                            VoucherID = model.VoucherID,
                            Accesstime = today,
                            Quantity = 1,
                            PaymentMethodID = model.PaymentMethodID,
                            PromoCodes = getPromoCodes(model.CustomerPhone),
                            Amount = model.Amount,
                            Discount = model.Discount,
                            TotalAmount = model.Discount != null ? model.Amount - ((model.Amount * model.Discount) / 100) : model.Amount
                            
                        };
                        tbpurchase updatedPurchase = uow.purchaseRepo.InsertReturn(purchase);
                        TotalAmount += updatedPurchase.TotalAmount;
                        PurchaseViewModel pvm = new PurchaseViewModel()
                        {
                            CustomerID = updatedCustomer.ID,
                            CustomerName = updatedCustomer.Name,
                            VoucherID = updatedPurchase.VoucherID,
                            VoucherName = voucher.Title,
                            Accesstime = updatedPurchase.Accesstime,
                            Quantity = 1,
                            PaymentMethodID = updatedPurchase.PaymentMethodID,
                            PaymentMethodName = model.PaymentMethodName,
                            PromoCodes = updatedPurchase.PromoCodes,
                            Amount = updatedPurchase.Amount,
                            Discount = updatedPurchase.Discount,
                            TotalAmount = updatedPurchase.TotalAmount,

                        };
                        plist.Add(pvm);
                    }
                  
                    if (voucher.UsedQuantity >= 0)
                    {
                        voucher.UsedQuantity += model.Quantity;
                    }

                    if (voucher.Quantity >= 0)
                    {
                        voucher.Quantity -= model.Quantity;
                    }

                    voucher = uow.voucherRepo.UpdateWithObj(voucher);

                    // payment gateway
                    var paymentIntents = new PaymentIntentService();
                    var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
                    {
                        Amount = Convert.ToInt64(TotalAmount),
                        Currency = "usd",
                    });

                    if (voucher != null)
                    {
                        rvm = new ReturnPaymentViewModel()
                        {
                            status = "Success",
                            message = "Payment Successful",
                            secretkey = paymentIntent.ClientSecret,
                            list = plist
                        };
                    }
                    else
                    {
                        rvm = new ReturnPaymentViewModel()
                        {
                            status = "Fail",
                            message = "Payment Fail",

                        };
                    }
                }
                catch (Exception ex)
                {
                    rvm = new ReturnPaymentViewModel()
                    {
                        status = "Fail",
                        message = ex.Message,

                    };
                }
            }
            else
            {
                rvm = new ReturnPaymentViewModel()
                {
                    status = "Quantity is not available.",
                    message = $"{availableqty} available",

                };
            }




            return Ok(rvm);
        }



        public string getPromoCodes(string phone)
        {
            var promocodes = MyExtension.generatePromoCode(phone);
            while (_db.purchases.Where(p => p.PromoCodes == promocodes).Select(p => p.ID).Count() > 0)
            {
                promocodes = MyExtension.generatePromoCode(phone);
            }
            return promocodes;
        }
    }
}
