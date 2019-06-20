using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoHistoryCore;
using DAL;

namespace Contracorpanel.Controllers
{

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
            var _TaxiServices = _context.TaxiServices.AsQueryable();
            _TaxiServices = _TaxiServices.Include(t => t.Driver);
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

        // GET: TaxiServices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiService = await _context.TaxiServices
                .Include(t => t.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taxiService == null)
            {
                return NotFound();
            }

            return View(taxiService);
        }

        // GET: TaxiServices/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Name");
            return View();
        }

        // POST: TaxiServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DriverId,TaxiCabState,DriverPercent,Hs_Change,IsDeleted")] TaxiService taxiService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxiService);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Name", taxiService.DriverId);
            return View(taxiService);
        }

        // GET: TaxiServices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiService = await _context.TaxiServices.FindAsync(id);
            if (taxiService == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "PropertyName", taxiService.DriverId);
            return View(taxiService);
        }

        // POST: TaxiServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,DriverId,TaxiCabState,DriverPercent,Hs_Change,IsDeleted")] TaxiService taxiService)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxiServiceExists(taxiService.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Name", taxiService.DriverId);
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
                .Include(t => t.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
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

        private bool TaxiServiceExists(string id)
        {
            return _context.TaxiServices.Any(e => e.Id == id);
        }
    }
}
