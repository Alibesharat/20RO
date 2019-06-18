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
    public class GeneralSettingsController : Controller
    {
        private readonly TaxiContext _context;

        public GeneralSettingsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: GeneralSettings
        public async Task<IActionResult> Index()
        {

            var data =await _context.GeneralSettings.ToListAsync();
            return View(data);
        }

        // GET: GeneralSettings/Details/5
      
        // GET: GeneralSettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GeneralSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SiteName,LogoPath,SeasionCount")] GeneralSetting generalSetting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(generalSetting);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            return View(generalSetting);
        }

        // GET: GeneralSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalSetting = await _context.GeneralSettings.FindAsync(id);
            if (generalSetting == null)
            {
                return NotFound();
            }
            return View(generalSetting);
        }

        // POST: GeneralSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SiteName,LogoPath,SeasionCount")] GeneralSetting generalSetting)
        {
            if (id != generalSetting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(generalSetting);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneralSettingExists(generalSetting.Id))
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
            return View(generalSetting);
        }

        // GET: GeneralSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var generalSetting = await _context.GeneralSettings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (generalSetting == null)
            {
                return NotFound();
            }

            return View(generalSetting);
        }

        // POST: GeneralSettings/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var generalSetting = await _context.GeneralSettings.FindAsync(id);
            _context.GeneralSettings.Remove(generalSetting);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool GeneralSettingExists(int id)
        {
            return _context.GeneralSettings.Any(e => e.Id == id);
        }
    }
}
