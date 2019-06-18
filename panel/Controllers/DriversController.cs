using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panel.Extention;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers
{

    [Authorize(Roles = nameof(RolName.Contractor))]
    public class DriversController : Controller
    {
        private readonly TaxiContext _context;

        public DriversController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index(int? ContractorId, int pageindex = 1, string searchterm = "")
        {
            var contractor = User.GetContractor();
            if (contractor == null || !ContractorId.HasValue)
                return Unauthorized();

            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _drivers = _context.Drivers.Undelited().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _drivers = _drivers.Where(c => c.Name.Contains(searchterm));
                ViewBag.searchterm = searchterm;
            }
           
                ContractorId = contractor.Id;

           
            _drivers = _drivers.Where(c => c.ContractorId == ContractorId);
            AllRouteData.Add(nameof(ContractorId), ContractorId.Value.ToString());
            count = _drivers.Count();
            _drivers = _drivers.Skip(SkipStep).Take(takeStep);
            ViewBag.AllRouteData = AllRouteData;
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _drivers.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        [Authorize(Roles = nameof(RolName.Contractor))]
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarName,CarType,CarColor,IranianIdCode,DrivingLicense,DriverCode,PelakNumber,CityId,IsMaried,HasPlan,Id,Name,LastName,Age,Gender,Password,PhoneNubmber,IsMobielVerifed,Email,IsEmailVerified,BirthDay,BeginDate,AllowActivity")] Driver driver, IFormFile AvatarPath)
        {
            var contractor = User.GetContractor();
            if (contractor == null)
                return Unauthorized();
            driver.ContractorId = contractor.Id;

            if (AvatarPath == null)
            {
                ModelState.AddModelError(nameof(AvatarPath), "تصویر راننده وارد شود");
            }
            if (AvatarPath != null && AvatarPath.Length > (500 * 1024))
            {
                ModelState.AddModelError(nameof(AvatarPath), "حجم تصویر نباید بیش از 500 کیلو بایت باشد");
            }
            if (ModelState.IsValid)
            {
                List<string> AllowedExtention = new List<string>()
                    {
                       ".jpg",
                       ".png"
                    };
                var data = await AlphaRest.File.SendFileAsync(AvatarPath, AllowedExtention, (500 * 1024), $"{Const.StudentparrentPath}/api/api/Getdriverpic");
                if (data != null && data != "false")
                {
                    driver.AvatarPath = data;
                }


                _context.Add(driver);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        [Authorize(Roles = nameof(RolName.Admin))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
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
        [Authorize(Roles = nameof(RolName.Admin))]
        public async Task<IActionResult> Edit(int id, [Bind("CarName,CarType,CarColor,IranianIdCode,DrivingLicense,DriverCode,PelakNumber,CityId,IsMaried,HasPlan,Id,Name,LastName,Age,Gender,Password,PhoneNubmber,IsMobielVerifed,Email,IsEmailVerified,BirthDay,BeginDate,AllowActivity,ContractorId")] Driver driver, IFormFile AvatarPath)
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
                    var data = await AlphaRest.File.SendFileAsync(AvatarPath, AllowedExtention, (500 * 1024), $"{Const.StudentparrentPath}/api/api/Getdriverpic");
                    if (!string.IsNullOrWhiteSpace(data) && data != "false")
                    {
                        driver.AvatarPath = data;
                    }
                }
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id))
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
            return View(driver);
        }

        // GET: Drivers/Delete/5
        [Authorize(Roles = nameof(RolName.Admin))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contractor = User.GetContractor();
            if (contractor == null)
                return Unauthorized();

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null || driver.ContractorId != contractor.Id)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RolName.Admin))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contractor = User.GetContractor();
            if (contractor == null)
                return Unauthorized();
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null || driver.ContractorId != contractor.Id)
            {
                return NotFound();
            }
            _context.Drivers.Remove(driver);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
