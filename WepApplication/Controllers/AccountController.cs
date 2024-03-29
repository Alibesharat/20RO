﻿using DAL;
using DAL.Contracts;
using DAL.ViewModels;
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

        private readonly ISMS<List<SendResult>> _notify;
        private readonly IDistributedCache _cache;
        public string CachedTimeUTC { get; set; }

        public AccountController(ISMS<List<SendResult>> notify, IDistributedCache cache)
        {
            _notify = notify;
            _cache = cache;
        }


        [HttpGet]

        public IActionResult UserChalenge()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserChalenge(string phoneNumber)
        {



            if (!phoneNumber.IsValidIranianMobileNumber())
            {
                ViewBag.msg = "شماره تلفن همراه معتبر نمی باشد";
                return View();
            }
            string token = Const.GeneratRandomNumber();
            await AddCashAsync(phoneNumber, token, 3);
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
                    PhoneNumber = phoneNumber,
                    Name = "user",
                    TelNumber = "00"

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
                await AddCashAsync("number", phoneNumber, 3);
                return RedirectToAction(nameof(Complete), model);
            }
            ViewBag.msg = "کد وارد شده معتبر نمی باشد";
            ViewBag.phoneNumber = phoneNumber;
            return View();
        }





        [AllowAnonymous]
        public async Task<IActionResult> Complete(RegisterStudentParrentViewModel model)
        {
            var s = _cache.GetString("number");
            if (string.IsNullOrWhiteSpace(s))
            {
                ModelState.AddModelError("", "اعتبار زمانی تمام شده است دوباره  ثبت نام کنید");
                return View(model);

            }
            model.PhoneNumber = s;
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



        [Authorize(Roles = nameof(RolName.Parrent))]
        [HttpGet]
        public IActionResult EditProfile()
        {
            var parrent = User.Getparrent();
            EditStudentParrentViewModel model = new EditStudentParrentViewModel()
            {
                Name = parrent.Name,
                TelNumber = parrent.telNumber
            };

            return View(model);
        }


        [Authorize(Roles = nameof(RolName.Parrent))]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditStudentParrentViewModel model)
        {
            var parrent = User.Getparrent();
            model.PhoneNubmber = parrent.PhoneNubmber;
            if (ModelState.IsValid)
            {

                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<StudentParent>>
             (model, Const.EditStudentParent, ApiMethode.Post);
                if (data == null)
                {

                    ModelState.AddModelError("", "ارتباط با سرور میسر نشد !");
                    return View(model);
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
                new AuthenticationProperties() { RedirectUri = "/Account/UserChalenge" },
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
            claimes.Add(new Claim(ClaimTypes.Name, data.Data.Name));
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
                           AllowRefresh = true,

                       });
        }


        private async Task AddCashAsync(string key, string value, int minute)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(minute)
            };
            await _cache.SetStringAsync(key, value, options);
        }

    }
}