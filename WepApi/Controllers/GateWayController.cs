using AlphaCoreLogger;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WepApi.Controllers
{
    public class GateWayController : Controller
    {
        private readonly TaxiContext _context;
        private readonly ICoreLogger _logger;

        public GateWayController(TaxiContext context, ICoreLogger logger)
        {
            _context = context;
            _logger = logger;
        }

       
    }
}