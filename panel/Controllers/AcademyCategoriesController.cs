using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers
{

   [Authorize(Roles = nameof(RolName.Admin))]
    public class AcademyCategoriesController : Controller
    {
        private readonly TaxiContext _context;

        public AcademyCategoriesController(TaxiContext context)
        {
            _context = context;
        }

        // GET: AcademyCategories
        public async Task<IActionResult> Index(int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            var _academyCategories = _context.AcademyCategories.Undelited().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _academyCategories = _academyCategories.Where(c => c.Name.Contains(searchterm));
                count = _academyCategories.Count();
                ViewBag.searchterm = searchterm;
            }
            else
            {
                count = _academyCategories.Count();
            }
            _academyCategories = _academyCategories.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _academyCategories.ToListAsync());
        }

        // GET: AcademyCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academyCategory = await _context.AcademyCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academyCategory == null)
            {
                return NotFound();
            }

            return View(academyCategory);
        }

        // GET: AcademyCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademyCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AcademyCategory academyCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academyCategory);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            return View(academyCategory);
        }

        // GET: AcademyCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academyCategory = await _context.AcademyCategories.FindAsync(id);
            if (academyCategory == null)
            {
                return NotFound();
            }
            return View(academyCategory);
        }

        // POST: AcademyCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] AcademyCategory academyCategory)
        {
            if (id != academyCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academyCategory);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademyCategoryExists(academyCategory.Id))
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
            return View(academyCategory);
        }

        // GET: AcademyCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academyCategory = await _context.AcademyCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academyCategory == null)
            {
                return NotFound();
            }

            return View(academyCategory);
        }

        // POST: AcademyCategories/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academyCategory = await _context.AcademyCategories.FindAsync(id);
            _context.AcademyCategories.Remove(academyCategory);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool AcademyCategoryExists(int id)
        {
            return _context.AcademyCategories.Undelited().Any(e => e.Id == id);
        }
    }
}
