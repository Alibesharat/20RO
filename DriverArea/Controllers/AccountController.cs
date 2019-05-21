using DAL;
using DNTPersianUtils.Core;
using DriverArea.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Shared.Contracts;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DriverArea.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var Driver = User.GetDriver();
            if (Driver != null)
            {
                return RedirectToLocal(ReturnUrl);
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginDriverViewModel model, string ReturnUrl)
        {
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<Driver>>
                (model, Const.Logindriver, ApiMethode.Post);
            if (data == null)
            {
                ViewBag.msg = "ارتباط با سرور میسر نشد !";
                return View();
            }

            if (data.statuse)
            {
                await AddAuthAsync(data);



                return RedirectToLocal(ReturnUrl);

            }
            ViewBag.msg = data.message;
            return View();



        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterDriverViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!model.PhoneNubmber.IsValidIranianMobileNumber())
            {
                ModelState.AddModelError(nameof(model.PhoneNubmber), "شماره موبایل  وارد شده معتبر نیست");
                return View(model);
            }
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<Driver>>
                (model, Const.Registerdriver, ApiMethode.Post);
            if (data == null)
            {

                ModelState.AddModelError("", "ارتباط با سرور میسر نشد !");
                return View();
            }
            if (data.statuse)
            {
                await AddAuthAsync(data);
                return RedirectToLocal("");
            }
            ModelState.AddModelError("", data.message);
            return View(model);



        }


        [HttpGet]
        public IActionResult ForgotPassowrd()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassowrd([FromForm]string phoneNumber)
        {
            ViewBag.msg = "فعلا امکان بازیابی رمز عبور وجود ندارد با پشتیبانی تماس بگیرد";
            return View();



        }


        public IActionResult Denied()
        {
            return View();
        }


        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties() { RedirectUri = "/Account/login" },
                CookieAuthenticationDefaults.AuthenticationScheme);

        }


        private IActionResult RedirectToLocal(string returnUrl)
        {

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);

            }

            return RedirectToAction("Activeservice", "Home");

        }


        private async Task AddAuthAsync(ResultContract<Driver> data)
        {
            var claimes = new List<Claim>();
            string userdata = JsonConvert.SerializeObject(data.Data);
            claimes.Add(new Claim(ClaimTypes.Name, data.Data.FullName));
            claimes.Add(new Claim(ClaimTypes.Role, RolName.Driver.ToString()));
            claimes.Add(new Claim(ClaimTypes.UserData, userdata));
            var ClaimIdentity = new ClaimsIdentity(RolName.Driver.ToString());
            ClaimIdentity.AddClaims(claimes);
            var userPrincipal = new ClaimsPrincipal(ClaimIdentity);
            await HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       userPrincipal,
                       new AuthenticationProperties
                       {
                           ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                           IsPersistent = false,
                           AllowRefresh = false
                       });
        }


    }
}