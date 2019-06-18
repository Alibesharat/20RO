using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;

namespace Panel.Controllers
{

   [Authorize(Roles = nameof(RolName.Admin))]
    public class PaymentsController : Controller
    {
        private readonly TaxiContext _context;

        public PaymentsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index(int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _payments = _context.payments.AsQueryable();
            _payments = _payments.Include(p => p.Parent).Include(p => p.ServiceRequset);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _payments = _payments.Where(c => c.TrackingCode.Contains(searchterm));
                ViewBag.searchterm = searchterm;

            }

            count = _payments.Count();
            ViewBag.AllRouteData = AllRouteData;
            _payments = _payments.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _payments.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments
                .Include(p => p.Parent)
                .Include(p => p.ServiceRequset)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["ParrentId"] = new SelectList(_context.StudentParents, "Id", "Name");
            ViewData["RequsetServiceId"] = new SelectList(_context.ServiceRequsets, "Id", "Name");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ammount,RequsetServiceId,ParrentId,TrackingCode,Success,Hs_Change,IsDeleted")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParrentId"] = new SelectList(_context.StudentParents, "Id", "Name", payment.ParrentId);
            ViewData["RequsetServiceId"] = new SelectList(_context.ServiceRequsets, "Id", "Name", payment.RequsetServiceId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["ParrentId"] = new SelectList(_context.StudentParents, "Id", "PropertyName", payment.ParrentId);
            ViewData["RequsetServiceId"] = new SelectList(_context.ServiceRequsets, "Id", "PropertyName", payment.RequsetServiceId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ammount,RequsetServiceId,ParrentId,TrackingCode,Success,Hs_Change,IsDeleted")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
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
            ViewData["ParrentId"] = new SelectList(_context.StudentParents, "Id", "Name", payment.ParrentId);
            ViewData["RequsetServiceId"] = new SelectList(_context.ServiceRequsets, "Id", "Name", payment.RequsetServiceId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments
                .Include(p => p.Parent)
                .Include(p => p.ServiceRequset)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.payments.FindAsync(id);
            _context.payments.Remove(payment);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.payments.Any(e => e.Id == id);
        }
    }
}
