using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web.ViewComponents
{
    public class Header : ViewComponent
    {


        static HttpClient httpClient = new HttpClient();
        [ResponseCache(Duration = 45000, Location = ResponseCacheLocation.Any)]
        public IViewComponentResult Invoke(int numberToTake)
        {


            return View();


        }
    }
}
