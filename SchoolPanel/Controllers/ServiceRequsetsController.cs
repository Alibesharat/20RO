using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panel.Extention;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolPanel.Controllers
{
    [Authorize(nameof(RolName.academy))]
    public class ServiceRequsetsController : Controller
    {
        private readonly TaxiContext _context;

        public ServiceRequsetsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: ServiceRequsets
        public async Task<IActionResult> Index(int pageindex = 1, string RequsetId = "", string IdCode = "", RequsetSate requsetSate = RequsetSate.AwaitingAcademy)
        {
            var Academyid = User.GetAcademy()?.Id;
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>
            {
                { nameof(requsetSate), requsetSate.ToString() }
            };


            var _ServiceRequsets = _context.ServiceRequsets.AsQueryable();
            _ServiceRequsets = _ServiceRequsets
                .Include(s => s.Accountings)
                .Where(c => c.AcademyId == Academyid
                && c.RequsetState == requsetSate);
            if (!string.IsNullOrWhiteSpace(RequsetId))
            {
                _ServiceRequsets = _ServiceRequsets
                    .Where(c => c.Id.Contains(RequsetId));
                ViewBag.RequsetId = RequsetId;
                AllRouteData.Add(nameof(RequsetId), RequsetId);
            }
            if (!string.IsNullOrWhiteSpace(IdCode))
            {
                _ServiceRequsets = _ServiceRequsets
                   .Where(c => c.IrIdCod.Contains(IdCode));
                ViewBag.IdCode = IdCode;
                AllRouteData.Add(nameof(IdCode), IdCode);
            }

            count = _context.ServiceRequsets.Count();
            _ServiceRequsets = _ServiceRequsets.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _ServiceRequsets.ToListAsync());
        }




        // GET: ServiceRequsets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequset = await _context.ServiceRequsets
                .Include(s => s.StudentParent)
                .Include(s => s.TaxiService)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequset == null)
            {
                return NotFound();
            }

            return View(serviceRequset);
        }






    }
}
