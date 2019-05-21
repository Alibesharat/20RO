using LiveMap.Models;
using DAL;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;


namespace LiveMap.Controllers
{

    public class HomeController : Controller
    {


        private readonly TaxiContext _context;

        public HomeController(TaxiContext context)
        {
            _context = context;
        }
        public IActionResult Index()
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
