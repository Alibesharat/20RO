using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panel.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracorpanel.Controllers
{
    [Authorize(nameof(RolName.Contractor))]
    public class DriversController : Controller
    {
        private readonly TaxiContext _context;

        public DriversController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index(int pageindex = 1, string PhoneNubmber = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            var _Drivers = _context.Drivers.Undelited().AsQueryable().Where(c => c.ContractorId == User.GetContractor().Id);
            if (!string.IsNullOrWhiteSpace(PhoneNubmber))
            {
                _Drivers = _Drivers.Where(c => c.PhoneNubmber.Contains(PhoneNubmber));
                ViewBag.PhoneNubmber = PhoneNubmber;
            }
            count = _Drivers.Count();
            _Drivers = _Drivers.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _Drivers.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.Undelited()
                .FirstOrDefaultAsync(m => m.Id == id && m.ContractorId == User.GetContractor().Id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarName,CarType,CarColor,IranianIdCode,DrivingLicense,DriverCode,PelakNumber,IsMaried,Id,Token,Name,PhoneNubmber,Password,BeginDate,AllowActivity")] Driver driver, IFormFile AvatarPath)
        {

            if (AvatarPath == null)
            {
                ModelState.AddModelError(nameof(AvatarPath), "تصویر راننده  باید وارد شود");
            }
            if (AvatarPath != null && AvatarPath.Length > (500 * 1024))
            {
                ModelState.AddModelError(nameof(AvatarPath), "حجم تصویر نباید بیش از 500 کیلو بایت باشد");
            }
            driver.ContractorId = User.GetContractor().Id;

            if (ModelState.IsValid)
            {
                try
                {
                    List<string> AllowedExtention = new List<string>()
                    {
                       ".jpg",
                       ".png"
                    };


                    var data = await AlphaRest.File.SendFileAsync(AvatarPath, AllowedExtention, (500 * 1024), $"{Const.DriverUploadFileApi}");
                    if (data != null && data != "false")
                    {
                        driver.AvatarPath = data;
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(AvatarPath),"تصویر آپلود نشد");
                        return View(driver);
                    }

                    _context.Add(driver);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                    return RedirectToAction(nameof(Details),new { id=driver.Id});
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null || driver.ContractorId != User.GetContractor().Id)
            {
                return NotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarName,CarType,CarColor,IranianIdCode,DrivingLicense,DriverCode,PelakNumber,IsMaried,Id,Name,PhoneNubmber,Password,BeginDate,AllowActivity")] Driver driver, IFormFile AvatarPath)
        {
            if (AvatarPath != null && AvatarPath.Length > (500 * 1024))
            {
                ModelState.AddModelError(nameof(AvatarPath), "حجم تصویر نباید بیش از 500 کیلو بایت باشد");
            }
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (AvatarPath != null)
                {
                    List<string> AllowedExtention = new List<string>()
                    {
                       ".jpg",
                       ".png"
                    };
                    var data = await AlphaRest.File.SendFileAsync(AvatarPath, AllowedExtention, (500 * 1024), $"{Const.DriverUploadFileApi}");
                    if (!string.IsNullOrWhiteSpace(data) && data != "false")
                    {
                        driver.AvatarPath = data;
                    }
                }
                try
                {
                    driver.ContractorId = User.GetContractor().Id;
                    _context.Update(driver);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException ex)
                {

                    throw ex;

                }
                return RedirectToAction(nameof(Details), new { id = driver.Id });
            }
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.Id == id && m.ContractorId == User.GetContractor().Id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver.ContractorId == User.GetContractor().Id)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }


    }
}
