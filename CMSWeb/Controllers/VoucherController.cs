using Data.Models;
using Data.ViewModels;
using Infra.Helper;
using Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSWeb.Controllers
{
    
    public class VoucherController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VoucherController(IWebHostEnvironment webHostEnvironment)
        {         
            _webHostEnvironment = webHostEnvironment;

        }


        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_token");
            if (token == null)
                return RedirectToAction("Index", "Login");
            return View();
        }

        public async Task<IActionResult> CheckOut(int ID)
        {
            var token = HttpContext.Session.GetString("_token");
            if (token == null)
                return RedirectToAction("Index", "Login");
            PaymentViewModel result = await VoucherApiRequestHelper.GetCheckOutData(ID, token);
            return View(result);
        }

        public async Task<IActionResult> CheckOutV2(int ID)
        {
            var token = HttpContext.Session.GetString("_token");
            if (token == null)
                return RedirectToAction("Index", "Login");
            PaymentViewModel result = await VoucherApiRequestHelper.GetCheckOutData(ID, token);
            return View(result);
        }


        public async Task<IActionResult> GetPaymentMethodList()
        {
            var token = HttpContext.Session.GetString("_token");
            List<tbpaymentmethod> result = await VoucherApiRequestHelper.GetPaymentMethodList(token);
            return Json(result);
        }

        public async Task<ActionResult> _list(int pagesize = 10, int page = 1)
        {
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            var token = HttpContext.Session.GetString("_token");
            PagedListClient<VoucherViewModel> result = await VoucherApiRequestHelper.List(pagesize, page, token);
            return PartialView("_list", result);
        }

        public ActionResult Add()
        {
            var token = HttpContext.Session.GetString("_token");
            if (token == null)
                return RedirectToAction("Index", "Login");
            ViewBag.Title = "Add";

            return View();
        }

        public ActionResult Edit(int ID = 0)
        {
            var token = HttpContext.Session.GetString("_token");
            if (token == null)
                return RedirectToAction("Index", "Login");
            ViewBag.ID = ID;
            return View();
        }


        public async Task<ActionResult> _form(string FormType, int ID)
        {
            tbvoucher data = new tbvoucher();
            ViewBag.FormType = FormType;
            if (FormType == "Add")
            {
               
                return PartialView("_form", data);
            }
            else
            {
                var token = HttpContext.Session.GetString("_token");
                tbvoucher result = await VoucherApiRequestHelper.GetById(ID,token);
                return PartialView("_form", result);

            }
        }


        [HttpPost]
        public async Task<ActionResult> upsert(tbvoucher data)
        {
            // create file path
            //string webRootPath = _webHostEnvironment.WebRootPath;
            //string contentRootPath = _webHostEnvironment.ContentRootPath;
            //string path = "";
            //path = Path.Combine(contentRootPath, "Voucher");
            var token = HttpContext.Session.GetString("_token");
            tbvoucher result = await VoucherApiRequestHelper.UpSert(data,token);

            if(result != null)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }

           

        }

        [HttpPost]
        public async Task<ActionResult> makePayment(PaymentViewModel data)
        {
            var token = HttpContext.Session.GetString("_token");
            ReturnPaymentViewModel result = await VoucherApiRequestHelper.makePayment(data, token);

            if (result != null)
            {
                return Json(result);
            }
            else
            {
                return Json("Fail");
            }

        }



        [HttpPost]
        public async Task<ActionResult> makeActualPayment(PaymentViewModel data)
        {
            var token = HttpContext.Session.GetString("_token");
            ReturnPaymentViewModel result = await VoucherApiRequestHelper.makeActualPayment(data, token);

            if (result != null)
            {
                return Json(result);
            }
            else
            {
                return Json("Fail");
            }

        }


        [HttpGet]
        public async Task<ActionResult> SetActive(int ID)
        {
            var token = HttpContext.Session.GetString("_token");
            tbvoucher result = await VoucherApiRequestHelper.SetActive(ID, token);

            if (result != null)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }



        }
    }
}
