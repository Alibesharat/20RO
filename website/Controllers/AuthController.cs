using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace website.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }



        public async Task<IActionResult> Login(string returnUrl = null)
        {

            var claimes = new List<Claim>();

            claimes.Add(new Claim(ClaimTypes.Name, "Ali"));
            var ClaimIdentity = new ClaimsIdentity("SuperSecureLogin");
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

            return RedirectToLocal(returnUrl);

        }


        public IActionResult LogOut()
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
    }
}