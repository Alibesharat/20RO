using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers
{
   [Authorize(Roles = nameof(RolName.Admin))]
    public class DistrictsController : Controller
    {
        private readonly TaxiContext _context;

        public DistrictsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Districts
        public async Task<IActionResult> Index(int? CityId, int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _districts = _context.Districts.Undelited().AsQueryable();
            _districts = _districts.Include(d => d.City);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _districts = _districts.Where(c => c.Name.Contains(searchterm));
                count = _districts.Count();
                ViewBag.searchterm = searchterm;
            }
            if (CityId.HasValue)
            {
                _districts = _districts.Where(c => c.CityId == CityId);
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", CityId.Value);
                AllRouteData.Add(nameof(CityId), CityId.ToString());
            }
            else
            {
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            }

            count = _districts.Count();
            ViewBag.AllRouteData = AllRouteData;
            _districts = _districts.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _districts.ToListAsync());
        }

        // GET: Districts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.Districts
                .Include(d => d.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // GET: Districts/Create
        public IActionResult Create(int? CityId)
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name",CityId);
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CityId")] District district)
        {
            if (ModelState.IsValid)
            {
                _context.Add(district);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", district.CityId);
            return View(district);
        }

        // GET: Districts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.Districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", district.CityId);
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CityId")] District district)
        {
            if (id != district.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", district.CityId);
            return View(district);
        }

        // GET: Districts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.Districts
                .Include(d => d.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: Districts/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var district = await _context.Districts.FindAsync(id);
            _context.Districts.Remove(district);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }
    }
}
