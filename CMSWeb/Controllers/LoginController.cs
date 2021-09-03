using Data.ViewModels;
using Infra.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Index(AuthModel model)
        {
            AuthResponseModel result = await LoginApiRequestHelper.authenticate(model);
            if(result != null)
            {
                HttpContext.Session.SetString("_token", result.Token);
                return RedirectToAction("Index", "Voucher");
            }
            else
            {
                ViewBag.status = "UserName or Password wrong.";
                return View();
            }
           
        }

    }
}
