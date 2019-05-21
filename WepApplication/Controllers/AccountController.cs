using DAL;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Shared.Contracts;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WepApplication.Util;

namespace WepApplication.Controllers
{
    [AllowAnonymous]
  
    public class AccountController : Controller
    {


     


        [HttpGet]
      
        public IActionResult Login(string ReturnUrl)
        {
            var parrent = User.Getparrent();
            if (parrent != null)
                return RedirectToLocal(ReturnUrl);
                ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]LoginStudentParrentViewModel model, string ReturnUrl)
        {
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<StudentParent>>
                (model, Const.LoginStudentParrent, ApiMethode.Post);
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
            else
            {
                ViewBag.msg = "نام کاربری یا رمز عبور اشتباه است";
                return View();
            }





        }



        [HttpGet]
      
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Register([FromForm]RegisterStudentParrentViewModel model)
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
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<StudentParent>>
                (model, Const.RegisterStudentParent, ApiMethode.Post);
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
            LoginStudentParrentViewModel lm = new LoginStudentParrentViewModel()
            {
                PhoneNubmber = phoneNumber,
                Password = ""
            };
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<bool>>
               (lm, Const.ForgotPassword, ApiMethode.Post);
            if (data != null && data.statuse == true)
            {
                ViewBag.msg = data.message;

            }
            else
            {
                ViewBag.msg = "مشکلی بوجود آمد";

            }

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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        private async Task AddAuthAsync(ResultContract<StudentParent> data)
        {
            var claimes = new List<Claim>();
            string userdata = JsonConvert.SerializeObject(data.Data);
            claimes.Add(new Claim(ClaimTypes.Name, data.Data.FullName));
            claimes.Add(new Claim(ClaimTypes.Role, RolName.Parrent.ToString()));
            claimes.Add(new Claim(ClaimTypes.UserData, userdata));
            var ClaimIdentity = new ClaimsIdentity(RolName.Parrent.ToString());
            ClaimIdentity.AddClaims(claimes);
            var userPrincipal = new ClaimsPrincipal(ClaimIdentity);
            await HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       userPrincipal,
                       new AuthenticationProperties
                       {
                           ExpiresUtc = DateTime.UtcNow.AddDays(15),
                           IsPersistent = true,
                           AllowRefresh = true
                       });
        }


        

    }
}