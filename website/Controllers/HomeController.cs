using website.Models;
using DAL;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace website.Controllers
{

    public class HomeController : Controller
    {
       



        public HomeController()
        {
           
        }
        public IActionResult Index()
        {

           
           
            return View();
        }

        [Route("سوالات_متدوال")]
        public IActionResult faq()
        {
            return View();
        }


      

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
