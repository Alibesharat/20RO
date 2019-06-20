using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panel.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolPanel.Controllers
{


    [Authorize(nameof(RolName.academy))]
    public class HomeController : Controller
    {
        private readonly TaxiContext _context;

        public HomeController(TaxiContext context)
        {
            _context = context;
        }


        // لیست درخواست ها
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


            var _ServiceRequsets = _context.ServiceRequsets.Undelited().AsQueryable();
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
                .FirstOrDefaultAsync(m => m.Id == id && m.AcademyId == User.GetAcademy().Id);
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
               .FirstOrDefaultAsync(m => m.Id == id && m.AcademyId == User.GetAcademy().Id);
            if (serviceRequset == null)
                return NotFound();
            return View(serviceRequset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //تغییر وضعیت درخواست
        public async Task<IActionResult> ChangeState(string id, RequsetSate requsetSate)
        {
            var serviceRequset = await _context.ServiceRequsets
               .FirstOrDefaultAsync(m => m.Id == id && m.AcademyId == User.GetAcademy().Id);
            if (serviceRequset == null)
                return NotFound();
            try
            {
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



        //لیست پرداخت ها
        public async Task<IActionResult> Accounting(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounting = await _context.Accountings.Undelited()
               .Where(c => c.ServiceRequsetId == id).ToListAsync();
            ViewBag.ServiceRequsetId = id;
            ViewData["Count"] = accounting.Count();
            return View(accounting);
        }



        //افزدون پرداخت
        public IActionResult AddAccounting(string id)
        {
            var academyId = User.GetAcademy()?.Id;
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            };


            if (!(_context.ServiceRequsets.Undelited().Any(c => c.Id == id && c.AcademyId == academyId)))
            {
                throw new Exception($" academy by Id :  [{academyId}] try to  AddAccounting out of his scope");
            }
            TempData["id"] = id;
            return View();
        }

        // POST: Accountings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //افزدون پرداخت
        public async Task<IActionResult> AddAccounting([Bind("Id,PayType,Payed,PayDate,NextPay,TrackNumber,Comment")] Accounting accounting)
        {
            var ServiceRequsetId = TempData["id"]?.ToString();
            if (!string.IsNullOrWhiteSpace(ServiceRequsetId))
                accounting.ServiceRequsetId = ServiceRequsetId;
            if (ModelState.IsValid)
            {
                _context.Add(accounting);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Accounting), ServiceRequsetId);
            }
            return View(accounting);
        }

        //ویرایش پرداخت
        public async Task<IActionResult> EditAccounting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounting = await _context.Accountings.FindAsync(id);
            if (accounting == null)
            {
                return NotFound();
            }
            return View(accounting);
        }

        // POST: Accountings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //ویرایش پرداخت
        public async Task<IActionResult> EditAccounting([Bind("Id,ServiceRequsetId,PayType,Payed,PayDate,NextPay,TrackNumber,Comment")] Accounting accounting)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    var acadmyId = User.GetAcademy().Id;
                    var serviceRequset = _context.ServiceRequsets.FirstOrDefault(c => c.Id == accounting.ServiceRequsetId && c.AcademyId == acadmyId);

                    if (serviceRequset == null)
                    {
                        throw new Exception($"acadmy by Id : [{acadmyId}] try to    EditAccounting out of his scope");
                    }
                    _context.Update(accounting);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                    return RedirectToAction(nameof(Accounting), serviceRequset.Id);

                }
                catch (DbUpdateConcurrencyException ex)
                {

                    throw ex;

                }
            }
            return View(accounting);
        }






    }
}
