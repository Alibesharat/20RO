using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoHistoryCore;
using DAL;

namespace SchoolPanel.Controllers
{

    public class AccountingsController : Controller
    {
        private readonly TaxiContext _context;

        public AccountingsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Accountings
        public async Task<IActionResult> Index(int pageindex=1,string searchterm="")
        {
            var takeStep =10;
            var SkipStep=(pageindex-1) * takeStep;
            int count=0;
            ViewBag.curent = pageindex;
              var _Accountings=_context.Accountings.AsQueryable();
                   _Accountings = _Accountings.Include(a => a.ServiceRequset);
            if(!string.IsNullOrWhiteSpace(searchterm)){
                        _Accountings = _Accountings.Where(c => c.Name.Contains(searchterm));
                        count = _Accountings.Count();
                        ViewBag.searchterm = searchterm;
            }
            else{
            count =_context.Accountings.Count();
            }
                _Accountings = _Accountings.Skip(SkipStep).Take(takeStep);
                ViewData["Count"]=count;
                ViewBag.pageCount = (count / takeStep) + 1;
                return View(await _Accountings.ToListAsync());
        }

        // GET: Accountings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounting = await _context.Accountings
                .Include(a => a.ServiceRequset)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounting == null)
            {
                return NotFound();
            }

            return View(accounting);
        }

        // GET: Accountings/Create
        public IActionResult Create()
        {
            ViewData["ServiceRequsetId"] = new SelectList(_context.ServiceRequsets, "Id", "Name");
            return View();
        }

        // POST: Accountings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceRequsetId,PayType,Payed,PayDate,NextPay,TrackNumber,Comment,Hs_Change,IsDeleted")] Accounting accounting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accounting);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceRequsetId"] = new SelectList(_context.ServiceRequsets, "Id", "Name", accounting.ServiceRequsetId);
            return View(accounting);
        }

        // GET: Accountings/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["ServiceRequsetId"] = new SelectList(_context.ServiceRequsets, "Id", "PropertyName", accounting.ServiceRequsetId);
            return View(accounting);
        }

        // POST: Accountings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceRequsetId,PayType,Payed,PayDate,NextPay,TrackNumber,Comment,Hs_Change,IsDeleted")] Accounting accounting)
        {
            if (id != accounting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accounting);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountingExists(accounting.Id))
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
            ViewData["ServiceRequsetId"] = new SelectList(_context.ServiceRequsets, "Id", "Name", accounting.ServiceRequsetId);
            return View(accounting);
        }

        // GET: Accountings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounting = await _context.Accountings
                .Include(a => a.ServiceRequset)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounting == null)
            {
                return NotFound();
            }

            return View(accounting);
        }

        // POST: Accountings/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accounting = await _context.Accountings.FindAsync(id);
            _context.Accountings.Remove(accounting);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool AccountingExists(int id)
        {
            return _context.Accountings.Any(e => e.Id == id);
        }
    }
}
