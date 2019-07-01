using AutoHistoryCore;
using DAL;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Panel.Extention;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers
{

    [Authorize(Roles = nameof(RolName.Admin))]
    public class ServiceRequsetsController : Controller
    {
        private readonly TaxiContext _context;

        public ServiceRequsetsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: ServiceRequsets
        public async Task<IActionResult> Index(int? ContractorId, RequsetSate RequsetSate, int pageindex = 1, string searchterm = "")
        {


            var contractor = User.GetAdmin();
          
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;


            var _serviceRequsets = _context.ServiceRequsets.Undelited().AsQueryable();
            _serviceRequsets = _serviceRequsets
                .Include(c => c.Academy)
                .Include(s => s.StudentParent)
                .Include(c => c.Accountings);
                
            _serviceRequsets = _serviceRequsets.Where(c => c.RequsetState == RequsetSate);
            ViewBag.selectedName = RequsetSate.GetDisplayName();
            ViewBag.selectedvalue = RequsetSate;
            var AllRouteData = new Dictionary<string, string>()
            {
                {"RequsetSate",RequsetSate.ToString() },
                {"searchterm",searchterm },
            };
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _serviceRequsets = _serviceRequsets.Where(c => c.StudentParent.PhoneNubmber.Contains(searchterm));
                ViewBag.searchterm = searchterm;
            }
            if (ContractorId.HasValue)
            {
                _serviceRequsets = _serviceRequsets.Where(c => c.Academy.ContractorId == ContractorId);
                AllRouteData.Add(nameof(ContractorId), ContractorId.Value.ToString());
            }
            ViewBag.AllRouteData = AllRouteData;
            count = _serviceRequsets.Count();

            _serviceRequsets = _serviceRequsets.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;

            return View(await _serviceRequsets.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequset == null)
            {
                return NotFound();
            }
            var changes = JsonConvert.DeserializeObject<ICollection<HistoryViewModel>>(serviceRequset.Hs_Change);
            ViewBag.date = changes.FirstOrDefault(c => c.State == "Added")?.DateTime.Value.ToPersianDateTextify();
            return View(serviceRequset);
        }

      
    

        // GET: ServiceRequsets/Edit/5
        [Authorize(Roles = nameof(RolName.Admin))]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequset = await _context.ServiceRequsets.FindAsync(id);
            if (serviceRequset == null)
            {
                return NotFound();
            }
            ViewData["StudentParrentId"] = new SelectList(_context.StudentParents, "Id", "Name", serviceRequset.StudentParrentId);
            return View(serviceRequset);
        }

        // POST: ServiceRequsets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RolName.Admin))]

        public async Task<IActionResult> Edit(string id, [Bind("Id,StudentParrentId,FullName,Age,Address,Note,CourseId,RequsetState")] ServiceRequset serviceRequset)
        {
            if (id != serviceRequset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequset);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                  
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentParrentId"] = new SelectList(_context.StudentParents, "Id", "Name", serviceRequset.StudentParrentId);
            return View(serviceRequset);
        }

        // GET: ServiceRequsets/Delete/5
        [Authorize(Roles = nameof(RolName.Admin))]

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequset = await _context.ServiceRequsets
                .Include(s => s.StudentParent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequset == null)
            {
                return NotFound();
            }

            return View(serviceRequset);
        }

        // POST: ServiceRequsets/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RolName.Admin))]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequset = await _context.ServiceRequsets.FindAsync(id);
            _context.ServiceRequsets.Remove(serviceRequset);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequsetExists(string id)
        {
            return _context.ServiceRequsets.Any(e => e.Id == id);
        }
    }
}
