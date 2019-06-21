using DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Panel.Extention;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private TaxiContext _context;
        public AuthController(TaxiContext context)
        {
            _context = context;
        }


        public IActionResult Login(string ReturnUrl)
        {
            var contractor = User.GetAdmin();
            if (contractor != null)
            {
                return RedirectToLocal(ReturnUrl);
            }
            ViewData["ReturnUrl"] = ReturnUrl;

            return View();
        }

     

        [HttpPost]
        public async Task<IActionResult> Login(string PhoneNumber, string Password)
        {


            Admin admin = await _context.Admins.FirstOrDefaultAsync(c => c.Username == PhoneNumber
               && c.Password == Password);
            if (admin != null)
            {
                var claimes = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{admin.Name}"),
                    new Claim(ClaimTypes.Role, nameof(RolName.Admin))
                };
               
                string userdata = JsonConvert.SerializeObject(admin);
                claimes.Add(new Claim(ClaimTypes.UserData, userdata));

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
            HttpContext.Response.Cookies.Delete("bisroAdmin");
            return RedirectToLocal("/");

        }

        private IActionResult RedirectToLocal(string returnUrl)
        {

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);

            }

            return RedirectToAction("index", "Home");

        }
    }
}
