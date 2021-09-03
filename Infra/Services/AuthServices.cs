//using Data.Models;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Text;

//namespace Infra.Services
//{
//    public class AuthServices
//    {
//        public void AuthorizeUser(tbStaff data, HttpContext context)
//        {
//            if (data != null)
//            {
//                var identity = new[] {
//                new Claim(ClaimTypes.Email, data.Email),
//                new Claim(ClaimTypes.Name,data.Username),
//                new Claim(ClaimTypes.GivenName, data.Name),
//                new Claim(ClaimTypes.Uri, data.Photo)
//            };

//                var claimsIdentity = new ClaimsIdentity(identity, CookieAuthenticationDefaults.AuthenticationScheme);
//                var principle = new ClaimsPrincipal();
//                principle.AddIdentity(claimsIdentity);

//                context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

//            }

//        }

//        public void LogoutUser(HttpContext context)
//        {
//            context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//        }
//    }
//}
