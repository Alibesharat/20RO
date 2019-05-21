﻿using AutoHistoryCore;
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
    public class CitiesController : Controller
    {
        private readonly TaxiContext _context;

        public CitiesController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index(int? ProvinceId, int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _cities = _context.cities.Undelited().AsQueryable();
            _cities = _cities.Include(c => c.Province);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _cities = _cities.Where(c => c.Name.Contains(searchterm));
                ViewBag.searchterm = searchterm;
            }
            if (ProvinceId.HasValue)
            {
                _cities= _cities.Where(c => c.ProvinceId == ProvinceId.Value);
                ViewData["ProvinceId"] = new SelectList(_context.provinces, "Id", "Name",ProvinceId.Value);
                AllRouteData.Add(nameof(ProvinceId), ProvinceId.ToString());
            }
            else
            {
                ViewData["ProvinceId"] = new SelectList(_context.provinces, "Id", "Name");
            }
            count = _cities.Count();
            ViewBag.AllRouteData = AllRouteData;
            _cities = _cities.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _cities.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.cities
                .Include(c => c.Province)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create(int? ProvinceId)
        {
            ViewData["ProvinceId"] = new SelectList(_context.provinces, "Id", "Name", ProvinceId);
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProvinceId,Hs_Change,IsDeleted")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinceId"] = new SelectList(_context.provinces, "Id", "Name", city.ProvinceId);
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["ProvinceId"] = new SelectList(_context.provinces, "Id", "Name", city.ProvinceId);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProvinceId,Hs_Change,IsDeleted")] City city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            ViewData["ProvinceId"] = new SelectList(_context.provinces, "Id", "Name", city.ProvinceId);
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.cities
                .Include(c => c.Province)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.cities.FindAsync(id);
            _context.cities.Remove(city);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.cities.Any(e => e.Id == id);
        }
    }
}
