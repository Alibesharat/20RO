using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoHistoryCore;
using DAL;

namespace Panel.Controllers
{

    public class DriverFactorsController : Controller
    {
        private readonly TaxiContext _context;

        public DriverFactorsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: DriverFactors
        public async Task<IActionResult> Index(int DriverId, int pageindex = 1)
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            var _driverFactors = _context.driverFactors.Undelited().AsQueryable();
            _driverFactors = _driverFactors.Include(d => d.ServiceRequset)
                .ThenInclude(se => se.Academy)
                .Include(d => d.TaxiCab)
                .ThenInclude(t => t.Driver)
                .Where(c => c.TaxiCab.DriverId == DriverId && c.TaxiCab.TaxiCabState==TaxiCabState.Ready);

            ViewBag.drivers = new SelectList(_context.drivers.Undelited(), "Id", "FullName", DriverId);
            count = _driverFactors.Count();

            _driverFactors = _driverFactors.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _driverFactors.ToListAsync());
        }

        // GET: DriverFactors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverFactor = await _context.driverFactors
                .Include(d => d.ServiceRequset)
                .Include(d => d.TaxiCab)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverFactor == null)
            {
                return NotFound();
            }

            return View(driverFactor);
        }

        // GET: DriverFactors/Create
        public IActionResult Create()
        {
            ViewData["serviceRequsetId"] = new SelectList(_context.serviceRequsets, "Id", "Name");
            ViewData["taxiCabeid"] = new SelectList(_context.taxiCabs, "Id", "Name");
            return View();
        }

        // POST: DriverFactors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,From,To,SeassionCount,taxiCabeid,serviceRequsetId,Hs_Change,IsDeleted")] DriverFactor driverFactor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driverFactor);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["serviceRequsetId"] = new SelectList(_context.serviceRequsets, "Id", "Name", driverFactor.serviceRequsetId);
            ViewData["taxiCabeid"] = new SelectList(_context.taxiCabs, "Id", "Name", driverFactor.taxiCabeid);
            return View(driverFactor);
        }

        // GET: DriverFactors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverFactor = await _context.driverFactors.FindAsync(id);
            if (driverFactor == null)
            {
                return NotFound();
            }
            ViewData["serviceRequsetId"] = new SelectList(_context.serviceRequsets, "Id", "PropertyName", driverFactor.serviceRequsetId);
            ViewData["taxiCabeid"] = new SelectList(_context.taxiCabs, "Id", "PropertyName", driverFactor.taxiCabeid);
            return View(driverFactor);
        }

        // POST: DriverFactors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,From,To,SeassionCount,taxiCabeid,serviceRequsetId,Hs_Change,IsDeleted")] DriverFactor driverFactor)
        {
            if (id != driverFactor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driverFactor);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverFactorExists(driverFactor.Id))
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
            ViewData["serviceRequsetId"] = new SelectList(_context.serviceRequsets, "Id", "Name", driverFactor.serviceRequsetId);
            ViewData["taxiCabeid"] = new SelectList(_context.taxiCabs, "Id", "Name", driverFactor.taxiCabeid);
            return View(driverFactor);
        }

        // GET: DriverFactors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverFactor = await _context.driverFactors
                .Include(d => d.ServiceRequset)
                .Include(d => d.TaxiCab)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driverFactor == null)
            {
                return NotFound();
            }

            return View(driverFactor);
        }

        // POST: DriverFactors/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driverFactor = await _context.driverFactors.FindAsync(id);
            _context.driverFactors.Remove(driverFactor);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool DriverFactorExists(int id)
        {
            return _context.driverFactors.Any(e => e.Id == id);
        }
    }
}
