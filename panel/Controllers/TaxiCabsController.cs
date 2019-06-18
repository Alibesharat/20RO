using AlphaCoreLogger;
using AutoHistoryCore;
using DAL;
using Kavenegar.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NotifCore;
using Panel.Extention;
using Panel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers
{

    [Authorize(Roles = nameof(RolName.Contractor))]
    public class TaxiSerivcesController : Controller
    {
        private readonly TaxiContext _context;
        private readonly ICoreLogger _logger;
        private ISMS<List<SendResult>> _sms;

        public TaxiSerivcesController(TaxiContext context, ICoreLogger logger, ISMS<List<SendResult>> sms)
        {
            _context = context;
            _logger = logger;
            _sms = sms;
        }

        // GET: TaxiSerivces
        public async Task<IActionResult> Index(int? ContractorId, int pageindex = 1, string searchterm = "")
        {
            var contractor = User.GetContractor();
           
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _TaxiSerivces = _context.TaxiServices.Undelited().AsQueryable();
            _TaxiSerivces = _TaxiSerivces.Include(t => t.Driver);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _TaxiSerivces = _TaxiSerivces.Where(c => c.Name.Contains(searchterm));
                ViewBag.searchterm = searchterm;
            }
            if (ContractorId.HasValue)
            {
                _TaxiSerivces = _TaxiSerivces.Where(c => c.Driver.ContractorId == contractor.Id);
                AllRouteData.Add(nameof(ContractorId), ContractorId.Value.ToString());
            }

            count = _TaxiSerivces.Count();
            _TaxiSerivces = _TaxiSerivces.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.AllRouteData = AllRouteData;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _TaxiSerivces.ToListAsync());

        }

        // GET: TaxiSerivces/Details/5
        public async Task<IActionResult> Details(string id)
        {
          
            var taxiCab = await _context.TaxiServices
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taxiCab == null)
            {
                return NotFound();
            }

            return View(taxiCab);
        }

        // GET: TaxiSerivces/Create
        public IActionResult Create()
        {
            var contractor = User.GetContractor();
            if (contractor != null)
            {
                return Unauthorized();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited().Where(c => c.ContractorId == contractor.Id), "Id", "FullName");
            return View();
        }

        // POST: TaxiSerivces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DriverId,DriverPercent")] TaxiService taxiCab)
        {
           
            var contractor = User.GetContractor();
            if (contractor != null)
            {
                return Unauthorized();
            }
   
            if (ModelState.IsValid)
            {
                _context.Add(taxiCab);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited().Where(c => c.ContractorId == contractor.Id), "Id", "FullName", taxiCab.DriverId);
            return View(taxiCab);
        }

        // GET: TaxiSerivces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiCab = await _context.TaxiServices.FindAsync(id);
            if (taxiCab == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited(), "Id", "FullName", taxiCab.DriverId);
            return View(taxiCab);
        }

        // POST: TaxiSerivces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,DriverId,TaxiSerivcestate,DriverPercent")] TaxiService taxiCab)
        {
            if (id != taxiCab.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxiCab);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                    if (taxiCab.TaxiCabState == TaxiCabState.Ready)
                    {

                        var txn = await _context.TaxiServices
                             .Include(c=>c.Passnegers)
                             .ThenInclude(p=>p.StudentParent)
                                      .FirstOrDefaultAsync(c => c.Id == taxiCab.Id);
                        if (txn != null)
                        {
                            
                            foreach (var item in txn.Passnegers)
                            {
                                await _sms.SendNotifyWithTemplateAsync(item.StudentParent.PhoneNubmber, "https://ilicar.ir/Home/ActiveSerive", MessageTemplate.ilicarbrief);
                            }

                        }

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!TaxiCabExists(taxiCab.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited(), "Id", "FullName", taxiCab.DriverId);
            return View(taxiCab);
        }

        // GET: TaxiSerivces/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiCab = await _context.TaxiServices
                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taxiCab == null)
            {
                return NotFound();
            }
            var PassengersCount = taxiCab.Passnegers.Count();
            if (PassengersCount > 0)
            {
                ViewBag.msg = "این سرویس دارای مسافر است لطفا ابتدا آن ها را خارج نمایید";
                return View(taxiCab);
            }


            return View(taxiCab);
        }

        // POST: TaxiSerivces/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taxiCab = await _context.TaxiServices
              .Include(t => t.Driver)
             
              .FirstOrDefaultAsync(m => m.Id == id);
            if (taxiCab == null)
            {
                return NotFound();
            }

            var PassengersCount =
         taxiCab.Passnegers?.Count();


       
            if (PassengersCount > 0)
            {

                ViewBag.msg = "این سرویس دارای مسافر است لطفا ابتدا آن ها را خارج نمایید";
                return View(nameof(Delete), taxiCab);
            }


            _context.TaxiServices.Remove(taxiCab);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> History(int? id)
        {
            if (id.HasValue)
            {
                var data = await _context.TaxiServices.FindAsync(id.Value);

                var s = JsonConvert.DeserializeObject<ICollection<HistoryViewModel>>(data.Hs_Change);
                return View(s.ToList());
            }
            return NotFound();
        }




        [HttpGet]
        public async Task<IActionResult> AddPassnger(string Id, int? academyid, string TaxiCabDrvier)
        {
            var contractor = User.GetContractor();
            if (contractor == null)
            {
                return Unauthorized();
            }
            
            var Passengers = await _context.ServiceRequsets
                .Include(cu => cu.Academy)
                .Where(c => c.RequsetState == RequsetSate.AwaitingAcademy).ToListAsync();
            if (academyid.HasValue)
            {
                Passengers = Passengers.Where(c => c.AcademyId == academyid).ToList();
            }
            Passengers = Passengers.Where(c => c.Academy.ContractorId == contractor.Id).ToList();
            var TaxiCab = await _context.TaxiServices
                .FirstOrDefaultAsync(c => c.Id == Id);
            if (TaxiCab == null) return NotFound();

            TaxiCabViewModel vm = new TaxiCabViewModel()
            {
                Passngers = Passengers,
                TaxiCab = TaxiCab
            };
            ViewBag.TaxiCabDrvier = TaxiCabDrvier;
            ViewData["Academy"] = new SelectList(_context.Academies.Undelited().ToList(), "Id", "Name", academyid);

            return View(vm);
        }

        /// <summary>
        /// ثبت مسافر بصورت ایجکسی
        /// </summary>
        /// <returns></returns>
        ///
        [HttpPost()]
        [Route("AddPassngerAjax")]
        public async Task<IActionResult> AddPassngerAjax([FromBody] PassengerViewModel model)
        {

            if (model == null)
                return Json(new { state = false, message = "مدلی یافت نشد" });
            var Passenger = await _context.ServiceRequsets.FindAsync(model.Requsetserviceid);
            if (Passenger == null)
            {
                return Json(new { state = false, message = "مسافری یافت نشد" });
            }
            if (Passenger.RequsetState != RequsetSate.AwaitingAcademy)
            {
                return Json(new { state = false, message = "این مسافر را نمی شود اضافه کرد" });
            }

            var Service = await _context.TaxiServices.ToListAsync();
            if (Service == null)
            {
                return Json(new { state = false, message = "سرویسی یافت نشد" });
            }
            
                return Json(new { state = true });
           
        }


        /// <summary>
        /// حذف مسافر بصورت ایجکسی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("RemovePassngerAjax")]
        public async Task<IActionResult> RemovePassngerAjax([FromBody] PassengerViewModel model)
        {
            if (model == null)
                return Json(new { state = false, message = "مدلی یافت نشد" });
            var Passenger = await _context.ServiceRequsets.FindAsync(model.Requsetserviceid);

            if (Passenger == null)
            {
                return Json(new { state = false, message = "مسافری یافت نشد" });
            }
            if (Passenger.RequsetState != RequsetSate.Servicing)
            {
                return Json(new { state = false, message = "این مسافر را نمی شود از تاکسی خارج کرد" });
            }


            var Service = await _context.TaxiServices.Undelited().Include(c => c.Passnegers).ToListAsync();

            if (Service == null)
            {
                return Json(new { state = false, message = "سرویسی یافت نشد" });
            }
            else
            {
              

          
                return Json(new { state = true, message = "مسافر از تاکسی خارج شد" });
            }
        }





        private bool TaxiCabExists(string id)
        {
            return _context.TaxiServices.Any(e => e.Id == id);
        }



      
    }

    public class PassengerViewModel
    {
        public int Requsetserviceid { get; set; }
        public int TaxicabId { get; set; }
    }

   
}

