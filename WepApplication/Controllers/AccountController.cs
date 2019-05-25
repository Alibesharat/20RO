using DAL;
using DNTPersianUtils.Core;
using Kavenegar.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NotifCore;
using Shared;
using Shared.Contracts;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WepApplication.Util;

namespace WepApplication.Controllers
{
    [AllowAnonymous]

    public class AccountController : Controller
    {

        private readonly ISMS<List<SendResult>> _notify;
        private readonly IDistributedCache _cache;
        public string CachedTimeUTC { get; set; }

        public AccountController(ISMS<List<SendResult>> notify, IDistributedCache cache)
        {
            _notify = notify;
            _cache = cache;
        }


        [HttpGet]

        public IActionResult UerChalenge()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UerChalenge(string phoneNumber)
        {



            if (!phoneNumber.IsValidIranianMobileNumber())
            {
                ViewBag.msg = "شماره تلفن همراه معتبر نمی باشد";
                return View();
            }
            string token = Const.GeneratRandomNumber();


            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };
            await _cache.SetStringAsync(phoneNumber, token, options);
            await _notify.SendNotifyWithTemplateAsync(phoneNumber, token, MessageTemplate.Bisroverify);
            return RedirectToAction(nameof(ValidateingNumber), new { phoneNumber });
        }

        public IActionResult ValidateingNumber(string phoneNumber)
        {
            ViewBag.phoneNumber = phoneNumber;
            ViewBag.newrequset = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateingNumber(string phoneNumber, string vierfiyCode)
        {
            var c = await _cache.GetStringAsync(phoneNumber);
            ViewBag.newrequset = false;
            if (c == null)
            {
                ViewBag.msg = "این کد منقضی شده است لطفا یک کد دیگر درخواست کنید";
                ViewBag.phoneNumber = phoneNumber;
                ViewBag.newrequset = true;
                return View();
            }
            if (c == vierfiyCode)
            {
                var model = new RegisterStudentParrentViewModel()
                {
                    PhoneNubmber = phoneNumber,
                    Name = "user",
                    LastName = "Name"

                };
                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<StudentParent>>
           (model, Const.IsExistStudentparrent, ApiMethode.Post);
                if (data == null)
                {

                    ViewBag.msg = "ارتباط با سرور برقرار نشد ، لطفا بعد امتحان کنید";
                    ViewBag.phoneNumber = phoneNumber;
                    ViewBag.newrequset = true;
                    return View();
                }
                if (data.statuse == true)
                {
                    await AddAuthAsync(data);
                    return RedirectToLocal("");
                }
                model.Name = "";
                model.LastName = "";
                return RedirectToAction(nameof(Complete), model);
            }
            ViewBag.msg = "کد وارد شده معتبر نمی باشد";
            ViewBag.phoneNumber = phoneNumber;
            return View();
        }

       



        [AllowAnonymous]
        public async Task<IActionResult> Complete(RegisterStudentParrentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<StudentParent>>
             (model, Const.RegisterStudentParent, ApiMethode.Post);
                if (data == null)
                {

                    ModelState.AddModelError("", "ارتباط با سرور میسر نشد !");
                    return View(model);
                }
                if (data.statuse)
                {
                    await AddAuthAsync(data);
                    return RedirectToLocal("");
                }
                ModelState.AddModelError("", data.message);
            }

            return View(model);
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