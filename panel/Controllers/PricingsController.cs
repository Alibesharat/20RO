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
    public class PricingsController : Controller
    {
        private readonly TaxiContext _context;

        public PricingsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Pricings
        public async Task<IActionResult> Index(int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _pricings = _context.pricings.Undelited().AsQueryable();
            _pricings = _pricings.Include(p => p.academy);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _pricings = _pricings.Where(c => c.Name.Contains(searchterm));

                ViewBag.searchterm = searchterm;
            }
            ViewBag.AllRouteData = AllRouteData;
            count = _pricings.Count();
            _pricings = _pricings.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _pricings.ToListAsync());
        }

        // GET: Pricings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricing = await _context.pricings
                .Include(p => p.academy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pricing == null)
            {
                return NotFound();
            }

            return View(pricing);
        }

        // GET: Pricings/Create
        public IActionResult Create()
        {
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name");
            return View();
        }

        // POST: Pricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AcademyId,FormKilometer,ToKilometer,ConstPrice,PricePerKilometer,Comission")] Pricing pricing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pricing);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", pricing.AcademyId);
            return View(pricing);
        }

        // GET: Pricings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricing = await _context.pricings.FindAsync(id);
            if (pricing == null)
            {
                return NotFound();
            }
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", pricing.AcademyId);
            return View(pricing);
        }

        // POST: Pricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AcademyId,FormKilometer,ToKilometer,ConstPrice,PricePerKilometer,Comission")] Pricing pricing)
        {
            if (id != pricing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricing);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricingExists(pricing.Id))
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
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", pricing.AcademyId);
            return View(pricing);
        }

        // GET: Pricings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricing = await _context.pricings
                .Include(p => p.AcademyId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pricing == null)
            {
                return NotFound();
            }

            return View(pricing);
        }

        // POST: Pricings/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pricing = await _context.pricings.FindAsync(id);
            _context.pricings.Remove(pricing);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool PricingExists(int id)
        {
            return _context.pricings.Any(e => e.Id == id);
        }
    }
}
