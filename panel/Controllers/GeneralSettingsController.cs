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
        public async Task<IActionResult> Edit(int id, [Bind("Id,SiteName,LogoPath,VanPercent,TaxiPercent")] GeneralSetting generalSetting)
        {
            if (id != generalSetting.Id)
            {
                return NotFound();
            }
            if (generalSetting.TaxiPercent <= 0)
                ModelState.AddModelError("TaxiPercent", "مقدار صفر برای قیمت پایه تاکسی سواری مجاز نیست");
            if (generalSetting.VanPercent <= 0)
                ModelState.AddModelError("VanPercent", "مقدار صفر برای قیمت پایه تاکسی ون مجاز نیست");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(generalSetting);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    
                        throw ex;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(generalSetting);
        }

      


    }
}
