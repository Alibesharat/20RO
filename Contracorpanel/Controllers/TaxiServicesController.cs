using AutoHistoryCore;
using DAL;
using DAL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panel.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracorpanel.Controllers
{
    [Authorize(Roles = nameof(RolName.Contractor))]
    public class TaxiServicesController : Controller
    {
        private readonly TaxiContext _context;

        public TaxiServicesController(TaxiContext context)
        {
            _context = context;
        }

        // GET: TaxiServices
        public async Task<IActionResult> Index(int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            var _TaxiServices = _context.TaxiServices.Undelited().AsQueryable();
            _TaxiServices = _TaxiServices
                .Include(t => t.Driver)
                .Where(c => c.Driver.ContractorId == User.GetContractor().Id);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _TaxiServices = _TaxiServices.Where(c => c.Name.Contains(searchterm));
                ViewBag.searchterm = searchterm;
            }

            count = _TaxiServices.Count();

            _TaxiServices = _TaxiServices.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _TaxiServices.ToListAsync());
        }



        // GET: TaxiServices/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited().Where(c => c.ContractorId == User.GetContractor().Id), "Id", "Name");
            return View();
        }

        // POST: TaxiServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DriverId,DriverPercent,ServiceType")] TaxiService taxiService)
        {
            if (ModelState.IsValid)
            {
                taxiService.TaxiCabState = TaxiCabState.wait;
                _context.Add(taxiService);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited().Where(c => c.ContractorId == User.GetContractor().Id), "Id", "Name", taxiService.DriverId);
            return View(taxiService);
        }

        // GET: TaxiServices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiService = await _context.TaxiServices.Undelited()
                .Include(c => c.Driver)
                .FirstOrDefaultAsync(c => c.Id == id && c.Driver.ContractorId == User.GetContractor().Id);
            if (taxiService == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited().Where(c => c.ContractorId == User.GetContractor().Id), "Id", "Name", taxiService.DriverId);
            return View(taxiService);
        }

        // POST: TaxiServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,DriverId,TaxiCabState,DriverPercent,ServiceType")] TaxiService taxiService)
        {
            if (id != taxiService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxiService);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException ex)
                {

                    throw ex;

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited().Where(c => c.ContractorId == User.GetContractor().Id), "Id", "Name", taxiService.DriverId);
            return View(taxiService);
        }

        // GET: TaxiServices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiService = await _context.TaxiServices
                .Include(c => c.Driver)
                .FirstOrDefaultAsync(m => m.Id == id && m.Driver.ContractorId == User.GetContractor().Id);
            if (taxiService == null)
            {
                return NotFound();
            }

            return View(taxiService);
        }

        // POST: TaxiServices/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taxiService = await _context.TaxiServices.FindAsync(id);
            _context.TaxiServices.Remove(taxiService);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }



        // GET: TaxiServices/Details/5
        public async Task<IActionResult> Passengers(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var taxiService = await _context.TaxiServices
                .Include(t => t.Driver)
                .Include(c => c.Passnegers)
                .FirstOrDefaultAsync(m => m.Id == id && m.Driver.ContractorId == User.GetContractor().Id);
            if (taxiService == null)
            {

                return NotFound();
            }
            ViewData["ServiceRequsets"] = new SelectList(_context.ServiceRequsets.Undelited()
                .Include(c => c.Academy)
                .Where(c => c.Academy.ContractorId == User.GetContractor().Id
                && c.TaxiServiceId == null)
                .OrderBy(c => c.Longtude).ThenBy(c => c.Latitue).ThenBy(c => c.Distance)
                , "Id", "FullName");

            return View(taxiService);
        }


        /// <summary>
        /// افزودن مسافر
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PassngerId"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPassenger(string Id, string PassngerId)
        {
            var TaxiService = await _context.TaxiServices.Undelited()
                .Include(t => t.Driver)
                .Include(c => c.Passnegers)
                .FirstOrDefaultAsync(c => c.Id == Id & c.Driver.ContractorId == User.GetContractor().Id);
            if (TaxiService == null)
            {
                return NotFound();
            }
            if (TaxiService.ServiceType == ServiceType.taxi && TaxiService.Passnegers.Count() >= 4)
            {
                ViewBag.msg = "برای خودروی سواری ، بیش از چهار مسافر مجاز نمی باشد";
                return View(nameof(Passengers), new { id = Id });
            }
            if (TaxiService.ServiceType == ServiceType.van && TaxiService.Passnegers.Count() >= 8)
            {
                ViewBag.msg = "برای خودروی ون ، بیش از هشت مسافر مجاز نمی باشد";
                return View(nameof(Passengers), new { id = Id });
            }
            var passnger = await _context.ServiceRequsets.Undelited().FirstOrDefaultAsync(c => c.Id == PassngerId);
            if (passnger == null)
            {
                ViewBag.msg = "مسافری با این مشخصات  یافت نشد";
                return View(nameof(Passengers), new { id = Id });
            }
            TaxiService.Passnegers.Add(passnger);
            _context.Update(TaxiService);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Passengers), new { id = Id });

        }



        /// <summary>
        /// حذف مسافر
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PassngerId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePassenger(string Id, string PassngerId)
        {
            var TaxiService = await _context.TaxiServices.Undelited()
                .Include(t => t.Driver)
                .Include(c => c.Passnegers)
                .FirstOrDefaultAsync(c => c.Id == Id & c.Driver.ContractorId == User.GetContractor().Id);
            if (TaxiService == null)
            {
                return NotFound();
            }

            var passnger = TaxiService.Passnegers.FirstOrDefault(c => c.Id == PassngerId);
            if (passnger == null)
            {
                ViewBag.msg = "مسافری با این مشخصات  یافت نشد";
                return View(nameof(Passengers), new { id = Id });
            }

            passnger.TaxiServiceId = null;
            _context.Entry(passnger).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Passengers), new { id = Id });

        }




        public async Task<IActionResult> ShowOnmap(string taxiServiceId)
        {
            var taxiServices = await _context.ServiceRequsets
                .Where(c => c.TaxiServiceId == taxiServiceId && c.Academy.ContractorId == User.GetContractor().Id)
                .ToListAsync();
            return View(taxiServices);
        }



        [HttpPost]
        [Route(nameof(Servicification))]
        public async Task<IActionResult> Servicification()
        {
            var contractorId = User.GetContractor().Id;
            var Drivers = _context.Drivers.Where(c => c.ContractorId == contractorId).ToList();
            if (Drivers == null || Drivers.Count <= 0)
            {
                return Json(new ResultContract<int>() { Data = 0, statuse = false, message = "هیچ راننده فعالی یافت نشد" });
            }

            int Count = _context.TaxiServices.Undelited().Include(c => c.Driver).Where(c => c.Driver.ContractorId == contractorId).Count();

            List<ServiceRequset> Requests = await _context.ServiceRequsets.Undelited()
                .Include(c => c.Academy)
                .Where(c => c.TaxiServiceId == null
                && c.RequsetState == RequsetSate.AwaitingContractor
                && c.Academy.ContractorId == contractorId)
                .OrderBy(c => c.Longtude).ThenBy(c => c.Latitue).ThenBy(c => c.Distance).ToListAsync();

            if (Requests == null || Requests.Count() <= 0)
            {
                return Json(new ResultContract<int>() { Data = 0, statuse = false, message = "هیچ درخواست سرویسی  یافت نشد" });

            }
            List<TaxiService> lst = new List<TaxiService>();
            var taxineeds = Requests.Where(c => c.ServiceType == ServiceType.taxi);
            var vanneeds = Requests.Where(c => c.ServiceType == ServiceType.van);
            for (int i = 0; i <= taxineeds.Count(); i += 4)
            {
                var reqs = taxineeds.Skip(i).Take(4);
                int DriverId = GetRandomDriver(Drivers);
                if (DriverId == 0)
                    break;
                TaxiService sr = new TaxiService()
                {
                    DriverId = DriverId,
                    DriverPercent = 75,
                    IsDeleted = false,
                    Name = $"سرویس شماره {(Count + 1)}",
                    ServiceType = ServiceType.taxi,
                    TaxiCabState = TaxiCabState.wait
                };
                foreach (var item in reqs)
                {
                    sr.Passnegers.Add(item);
                }
                lst.Add(sr);
                Count += 1;

            }
            for (int i = 0; i <= vanneeds.Count(); i += 8)
            {
                var reqs = vanneeds.Skip(i).Take(8);
                int DriverId = GetRandomDriver(Drivers);
                if (DriverId == 0)
                    break;
                TaxiService sr = new TaxiService()
                {
                    DriverId = DriverId,
                    DriverPercent = 75,
                    IsDeleted = false,
                    Name = $"سرویس شماره {(Count + 1)}",
                    ServiceType = ServiceType.van,
                    TaxiCabState = TaxiCabState.wait
                };
                foreach (var item in reqs)
                {
                    sr.Passnegers.Add(item);
                }
                lst.Add(sr);
                Count += 1;

            }


            await _context.TaxiServices.AddRangeAsync(lst);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return Json(new ResultContract<int>() { Data = 1, statuse = true, message = $"تعداد {lst.Count} سرویس با موفقیت  تنظیم شد" });




        }

        private int GetRandomDriver(List<Driver> drivers)
        {
            var count = drivers.Count();
            if (count <= 0) return 0;
            Random r = new Random();
            var res = r.Next(count);
            var id = drivers[res].Id;
            drivers.Remove(drivers[res]);
            return id;
        }
    }
}
