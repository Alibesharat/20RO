using DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AuthController : Controller
    {

        public AuthController()
        {

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string PhoneNumber, string Password)
        {


            int a = 5;
            if (a == 5)
            {
                var claimes = new List<Claim>();
                claimes.Add(new Claim(ClaimTypes.Name, $"Ali"));
                claimes.Add(new Claim(ClaimTypes.Role, nameof(RolName.academy)));

                //string userdata = JsonConvert.SerializeObject(admin);
                //claimes.Add(new Claim(ClaimTypes.UserData, userdata));

                var ClaimIdentity = new ClaimsIdentity(claimes,
                   CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddDays(15)
                };
                return SignIn(new ClaimsPrincipal(ClaimIdentity), properties, CookieAuthenticationDefaults.AuthenticationScheme);
            }
            ViewBag.msg = "نام کاربری یا رمز عبور اشتباه است";
            return View();


        }


        //public IActionResult AddAdmin()
        //{
        //    try
        //    {
        //        Admin ad = new Admin()
        //        {
        //            AllowActivity = true,
        //            Gender = false,
        //            IsMobielVerifed = true,
        //            Name = "مدیر",
        //            LastName = "سیستم",
        //            Password = "",
        //            PhoneNubmber = ""

        //        };
        //        _context.admins.Add(ad);
        //        _context.SaveChangesWithHistoryAsync(HttpContext);
        //        return Json("Sucess");
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(ex);
        //    }

        //}


        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("taxi");
            return RedirectToAction(nameof(Login));

        }
    }
}
