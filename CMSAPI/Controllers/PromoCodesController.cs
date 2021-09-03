using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using Data.ViewModels;
using Infra.Services;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using MoreLinq;

namespace CMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodesController : ControllerBase
    {
        public readonly ApplicationDbContext _db;
        UnitOfWork uow;
        public PromoCodesController(ApplicationDbContext db)
        {
            _db = db;
            this.uow = new UnitOfWork(_db);
        }



        [Route("VerifyPromoCodes")]
        [HttpPost]
        public IActionResult VerifyPromoCode(VerifyViewModel data)
        {
            ReturnVerifyPromocodesViewModel rvm = new ReturnVerifyPromocodesViewModel();
            var voucher = _db.purchases.FirstOrDefault(a => a.IsDeleted != true && a.PromoCodes == data.PromoCodes);
            if (voucher != null)
            {
                rvm = new ReturnVerifyPromocodesViewModel()
                {
                    status = "Success",
                    message = "Verify Success!",
                    promocodes = voucher.PromoCodes
                };
            }
            else
            {
                rvm = new ReturnVerifyPromocodesViewModel()
                {
                    status = "Fail",
                    message = "Verify Fail!"
                };
            }

            return Ok(rvm);
        }


        [Route("CheckOutbyEvoucher")]
        [HttpPost]
        public IActionResult CheckOutbyEvoucher(CheckOutbyEVoucherViewModel data)
        {
            // promocodes check
            ReturnVerifyPromocodesViewModel rvm = new ReturnVerifyPromocodesViewModel();
            var UpdatedEntity = new tbpurchase();
            var purchase = _db.purchases.FirstOrDefault(a => a.IsDeleted != true && a.PromoCodes == data.PromoCodes);
            if(purchase != null)
            {
                purchase.IsUsed = true;
                UpdatedEntity = uow.purchaseRepo.UpdateWithObj(purchase);
                rvm = new ReturnVerifyPromocodesViewModel()
                {
                    status = "Success",
                    message = "Checkout success with evoucher!",
                    promocodes = purchase.PromoCodes
                };
            }
            else
            {
                rvm = new ReturnVerifyPromocodesViewModel()
                {
                    status = "Fail",
                    message = "Promocodes already used!",
                    
                };
            }

            return Ok(rvm);
        }



    }
}
