using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Panel.Controllers
{

   [Authorize(Roles = nameof(RolName.Admin))]
    public class AcademiesController : Controller
    {
        private readonly TaxiContext _context;

        public AcademiesController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Academies
        public async Task<IActionResult> Index(int? DistictId, int? ContractorId, int pageindex = 1, string searchterm = "")
        {


            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();
            ViewBag.curent = pageindex;
            var _academies = _context.Academies.Undelited().AsQueryable();
            _academies = _academies.Include(a => a.Category).Include(a => a.District).Include(c => c.Contractor);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _academies = _academies.Where(c => c.Name.Contains(searchterm));
                count = _academies.Count();
                ViewBag.searchterm = searchterm;
            }
            if (DistictId.HasValue)
            {
                _academies = _academies.Where(c => c.DistrictId == DistictId);
                AllRouteData.Add(nameof(DistictId), DistictId.ToString());
            }
            if (ContractorId.HasValue)
            {
                _academies = _academies.Where(c => c.ContractorId == ContractorId);
                AllRouteData.Add(nameof(ContractorId), ContractorId.ToString());


            }
            count = _academies.Count();
            ViewBag.AllRouteData = AllRouteData;
            _academies = _academies.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _academies.ToListAsync());
        }

        // GET: Academies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academy = await _context.Academies
                .Include(a => a.Category)
                .Include(a => a.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academy == null)
            {
                return NotFound();
            }

            return View(academy);
        }

        // GET: Academies/Create
        public IActionResult Create()
        {
            ViewData["AcademyCategoryId"] = new SelectList(_context.AcademyCategories, "Id", "Name");
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            ViewData["ContractorId"] = new SelectList(_context.Contractors, "Id", "Name");
            return View();
        }

        // POST: Academies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address,DistrictId,AcademyCategoryId,SupportNumber,Id,Name,Password,PhoneNubmber,AllowActivity,ContractorId,AcademyPercent")] Academy academy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academy);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AcademyCategoryId"] = new SelectList(_context.AcademyCategories, "Id", "Name", academy.AcademyCategoryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", academy.DistrictId);
            ViewData["ContractorId"] = new SelectList(_context.Contractors, "Id", "FullName");

            return View(academy);
        }

        // GET: Academies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academy = await _context.Academies.FindAsync(id);
            if (academy == null)
            {
                return NotFound();
            }
            ViewData["AcademyCategoryId"] = new SelectList(_context.AcademyCategories, "Id", "Name", academy.AcademyCategoryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", academy.DistrictId);
            ViewData["ContractorId"] = new SelectList(_context.Contractors, "Id", "FullName", academy.ContractorId);

            return View(academy);
        }

        // POST: Academies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Address,DistrictId,AcademyCategoryId,SupportNumber,Id,Name,Password,PhoneNubmber,AllowActivity,ContractorId,AcademyPercent")] Academy academy)
        {
            if (id != academy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academy);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademyExists(academy.Id))
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
            ViewData["AcademyCategoryId"] = new SelectList(_context.AcademyCategories, "Id", "Name", academy.AcademyCategoryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", academy.DistrictId);
            ViewData["ContractorId"] = new SelectList(_context.Contractors, "Id", "FullName", academy.ContractorId);

            return View(academy);
        }


        public async Task<IActionResult> Geo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academy = await _context.Academies.FindAsync(id);
            if (academy == null)
            {
                return NotFound();
            }

            return View(academy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Geo(int? id, string Longtude, string latitude)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academy = await _context.Academies.FindAsync(id);
            if (academy == null)
            {
                return NotFound();
            }
            academy.Latitude = latitude;
            academy.Longtude = Longtude;
            try
            {
                _context.Update(academy);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcademyExists(academy.Id))
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



        // GET: Academies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academy = await _context.Academies
                .Include(a => a.Category)
                .Include(a => a.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academy == null)
            {
                return NotFound();
            }

            return View(academy);
        }

        // POST: Academies/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academy = await _context.Academies.FindAsync(id);
            _context.Academies.Remove(academy);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool AcademyExists(int id)
        {
            return _context.Academies.Any(e => e.Id == id);
        }
    }
}
