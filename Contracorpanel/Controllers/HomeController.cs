using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panel.Extention;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{

    [Authorize(Roles = nameof(RolName.Contractor))]
    public class HomeController : Controller
    {
        private readonly TaxiContext _context;
        public HomeController(TaxiContext context)
        {
            _context = context;
        }
        // لیست درخواست ها
        public async Task<IActionResult> Index(int? AcademyId, int pageindex = 1, string RequsetId = "", string IdCode = "", RequsetSate requsetSate = RequsetSate.AwaitingContractor)
        {
            var ContractorId = User.GetContractor()?.Id;
            var acadmeys = new SelectList(_context.Academies.Undelited().Where(c => c.ContractorId == ContractorId), "Id", "Name", AcademyId);
            ViewData["academys"] = acadmeys;
            var acadmyIds = acadmeys.Select(c => c.Value).ToList();

            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>
            {
                { nameof(requsetSate), requsetSate.ToString() },

            };

            ViewBag.selectedName = requsetSate.GetDisplayName();
            ViewBag.selectedvalue = requsetSate;
            var _ServiceRequsets = _context.ServiceRequsets.Undelited().AsQueryable();
            _ServiceRequsets = _ServiceRequsets
                .Include(s => s.Accountings)
                .Include(s => s.Academy)
                .Where(c => acadmyIds.Contains(c.AcademyId.ToString())
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
            if (AcademyId.HasValue)
            {
                _ServiceRequsets = _ServiceRequsets
                 .Where(c => c.AcademyId == AcademyId);
                ViewBag.AcademyId = AcademyId;
                AllRouteData.Add(nameof(AcademyId), AcademyId.ToString());
            }

            count = _ServiceRequsets.Count();
            _ServiceRequsets = _ServiceRequsets.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _ServiceRequsets.ToListAsync());
        }

        //جزییات یک درخواست
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequset = await _context.ServiceRequsets
                .Include(s => s.StudentParent)
                .Include(s => s.Academy)
                .FirstOrDefaultAsync(m => m.Id == id && m.Academy.ContractorId == User.GetContractor().Id);
            if (serviceRequset == null)
            {
                return NotFound();
            }

            return View(serviceRequset);
        }


        //تغییر وضعیت درخواست
        public async Task<IActionResult> ChangeState(string id)
        {
            var serviceRequset = await _context.ServiceRequsets
                .Include(c => c.Academy)
               .FirstOrDefaultAsync(m => m.Id == id && m.Academy.ContractorId == User.GetContractor().Id);
            if (serviceRequset == null || serviceRequset.RequsetState == RequsetSate.AwaitingAcademy)
                return NotFound();

            return View(serviceRequset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //تغییر وضعیت درخواست
        public async Task<IActionResult> ChangeState(string id, RequsetSate requsetSate)
        {

            var serviceRequset = await _context.ServiceRequsets
               .Include(c => c.Academy)
              .FirstOrDefaultAsync(m => m.Id == id && m.Academy.ContractorId == User.GetContractor().Id);
            if (serviceRequset == null || serviceRequset.RequsetState == RequsetSate.AwaitingAcademy)
                return NotFound();
            try
            {
                if (requsetSate == RequsetSate.AwaitingAcademy)
                {
                    ViewBag.msg = "ثبت این وضعیت به عهده مدرسه است";
                    return View(serviceRequset);
                }
                serviceRequset.RequsetState = requsetSate;
                _context.Update(serviceRequset);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                ViewBag.msg = "تغییر وضعیت با موفقیت انجام شد";
                return View(serviceRequset);
            }
            catch (DbUpdateException ex)
            {

                throw ex;
            }


        }


        //لیست پرداخت هایی یک سرویس  
        public async Task<IActionResult> Accounting(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounting = await _context.Accountings.Undelited()
               .Where(c => c.ServiceRequsetId == id).ToListAsync();
            ViewBag.FullName = _context.ServiceRequsets.Find(id).FullName;
            ViewData["Count"] = accounting.Count();
            return View(accounting);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
