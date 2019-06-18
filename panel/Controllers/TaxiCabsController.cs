using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoHistoryCore;
using DAL;
using AlphaCoreLogger;
using Panel.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using NotifCore;
using Kavenegar.Core.Models;
using DNTPersianUtils.Core;
using Panel.Extention;

namespace Panel.Controllers
{

    [Authorize(Roles = nameof(RolName.Contractor))]
    public class TaxiCabsController : Controller
    {
        private readonly TaxiContext _context;
        private readonly ICoreLogger _logger;
        private ISMS<List<SendResult>> _sms;

        public TaxiCabsController(TaxiContext context, ICoreLogger logger, ISMS<List<SendResult>> sms)
        {
            _context = context;
            _logger = logger;
            _sms = sms;
        }

        // GET: TaxiCabs
        public async Task<IActionResult> Index(int? ContractorId, int pageindex = 1, string searchterm = "")
        {
            var contractor = User.GetContractor();
            if (!contractor.IsCenterAdmin)
            {
                ContractorId = ContractorId.Value;
            }
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _taxiCabs = _context.TaxiServices.Undelited().AsQueryable();
            _taxiCabs = _taxiCabs.Include(t => t.Driver);
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _taxiCabs = _taxiCabs.Where(c => c.Name.Contains(searchterm));
                ViewBag.searchterm = searchterm;
            }
            if (ContractorId.HasValue)
            {
                _taxiCabs = _taxiCabs.Where(c => c.Driver.ContractorId == contractor.Id);
                AllRouteData.Add(nameof(ContractorId), ContractorId.Value.ToString());
            }

            count = _taxiCabs.Count();
            _taxiCabs = _taxiCabs.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.AllRouteData = AllRouteData;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _taxiCabs.ToListAsync());

        }

        // GET: TaxiCabs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiCab = await _context.TaxiServices
                .Include(t => t.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taxiCab == null)
            {
                return NotFound();
            }

            return View(taxiCab);
        }

        // GET: TaxiCabs/Create
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

        // POST: TaxiCabs/Create
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

        // GET: TaxiCabs/Edit/5
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

        // POST: TaxiCabs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DriverId,TaxiCabState,DriverPercent")] TaxiService taxiCab)
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
                             .Include(c => c.FirstPassnger)
                              .ThenInclude(c => c.studentParent)

                             .Include(c => c.SecondPassnger)
                               .ThenInclude(cs => cs.studentParent)

                             .Include(c => c.ThirdPassnger)
                              .ThenInclude(ct => ct.studentParent)
                             .Include(c => c.FourthPassnger)
                                .ThenInclude(c => c.studentParent)
                                      .FirstOrDefaultAsync(c => c.Id == taxiCab.Id);
                        if (txn != null)
                        {
                            var firstPassenger = txn.FirstPassnger.FirstOrDefault();
                            var secondePassenger = txn.SecondPassnger.FirstOrDefault();
                            var thirdPassengers = txn.ThirdPassnger.FirstOrDefault();
                            var forthPassengers = txn.FourthPassnger.FirstOrDefault();
                            List<string> numbers = new List<string>()
                            {
                               firstPassenger?.studentParent?.PhoneNubmber,
                               secondePassenger?.studentParent?.PhoneNubmber,
                               thirdPassengers?.studentParent?.PhoneNubmber,
                               forthPassengers?.studentParent?.PhoneNubmber

                            };
                            foreach (var item in numbers)
                            {
                                await _sms.SendNotifyWithTemplateAsync(item, "https://ilicar.ir/Home/ActiveSerive", MessageTemplate.ilicarbrief);
                            }

                        }

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxiCabExists(taxiCab.Id))
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
            ViewData["DriverId"] = new SelectList(_context.Drivers.Undelited(), "Id", "FullName", taxiCab.DriverId);
            return View(taxiCab);
        }

        // GET: TaxiCabs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxiCab = await _context.TaxiServices
                .Include(t => t.Driver)
                .Include(c => c.FirstPassnger)
                .Include(c => c.SecondPassnger)
                .Include(c => c.ThirdPassnger)
                .Include(c => c.FourthPassnger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taxiCab == null)
            {
                return NotFound();
            }
            var PassengersCount =
       taxiCab.FirstPassnger?.Count() +
       taxiCab.SecondPassnger?.Count() +
       taxiCab.ThirdPassnger?.Count() +
       taxiCab.FourthPassnger?.Count();
            if (PassengersCount > 0)
            {
                ViewBag.msg = "این سرویس دارای مسافر است لطفا ابتدا آن ها را خارج نمایید";
                return View(taxiCab);
            }


            return View(taxiCab);
        }

        // POST: TaxiCabs/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxiCab = await _context.TaxiServices
              .Include(t => t.Driver)
              .Include(c => c.FirstPassnger)
              .Include(c => c.SecondPassnger)
              .Include(c => c.ThirdPassnger)
              .Include(c => c.FourthPassnger)
              .FirstOrDefaultAsync(m => m.Id == id);
            if (taxiCab == null)
            {
                return NotFound();
            }

            var PassengersCount =
         taxiCab.FirstPassnger?.Count() +
         taxiCab.SecondPassnger?.Count() +
         taxiCab.ThirdPassnger?.Count() +
         taxiCab.FourthPassnger?.Count();
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
        public async Task<IActionResult> AddPassnger(int? Id, int? academyid, string TaxiCabDrvier)
        {
            var contractor = User.GetContractor();
            if (contractor == null)
            {
                return Unauthorized();
            }
            if (!Id.HasValue)
                return NotFound();
            var Passengers = await _context.ServiceRequsets
                .Include(cu => cu.Academy)
                .Where(c => c.RequsetState == RequsetSate.pending).ToListAsync();
            if (academyid.HasValue)
            {
                Passengers = Passengers.Where(c => c.AcademyId == academyid).ToList();
            }
            Passengers = Passengers.Where(c => c.Academy.ContractorId == contractor.Id).ToList();
            var TaxiCab = await _context.TaxiServices
                .Include(c => c.FirstPassnger).ThenInclude(co => co.Academy)
                .Include(c => c.SecondPassnger).ThenInclude(co => co.Academy)
                .Include(c => c.ThirdPassnger).ThenInclude(co => co.Academy)
                .Include(c => c.FourthPassnger).ThenInclude(co => co.Academy)
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
            if (Passenger.RequsetState != RequsetSate.pending)
            {
                return Json(new { state = false, message = "این مسافر را نمی شود اضافه کرد" });
            }

            var Service = await _context.TaxiServices.Include(c => c.FirstPassnger).Include(c => c.SecondPassnger).Include(c => c.ThirdPassnger).Include(c => c.FourthPassnger).FirstOrDefaultAsync(c => c.Id == model.TaxicabId);
            if (Service == null)
            {
                return Json(new { state = false, message = "سرویسی یافت نشد" });
            }

            else if (Service.IsCompelete)
            {

                return Json(new { state = false, message = "این سرویس تکمیل شده است" });
            }
            else
            {

                if (Service.FirstPassnger != null && Service.FirstPassnger.Count() <= 0)
                {
                    try
                    {
                        Service.FirstPassnger.Add(Passenger);
                        Passenger.RequsetState = RequsetSate.Servicing;
                        ChangeServiceState(Service);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {
                        await _logger.LogAsync(ex);
                        return Json(new { state = false, message = "خطایی بوجود آمد" });

                    }

                }
                else
                if (Service.SecondPassnger != null && Service.SecondPassnger.Count() <= 0)
                {
                    try
                    {
                        Service.SecondPassnger.Add(Passenger);
                        Passenger.RequsetState = RequsetSate.Servicing;
                        ChangeServiceState(Service);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {

                        await _logger.LogAsync(ex);
                        return Json(new { state = false, message = "خطایی بوجود آمد" });
                    }

                }
                else
                if (Service.ThirdPassnger != null && Service.ThirdPassnger.Count() <= 0)
                {
                    try
                    {
                        Service.ThirdPassnger.Add(Passenger);
                        Passenger.RequsetState = RequsetSate.Servicing;
                        ChangeServiceState(Service);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {

                        await _logger.LogAsync(ex);
                        return Json(new { state = false, message = "خطایی بوجود آمد" });
                    }

                }
                else
                if (Service.FourthPassnger != null && Service.FourthPassnger.Count() <= 0)
                {
                    try
                    {
                        Service.FourthPassnger.Add(Passenger);
                        Passenger.RequsetState = RequsetSate.Servicing;
                        ChangeServiceState(Service);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {

                        await _logger.LogAsync(ex);
                        return Json(new { state = false, message = "خطایی بوجود آمد" });
                    }

                }
                return Json(new { state = true });
            }
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


            var Service = await _context.TaxiServices.Include(c => c.FirstPassnger).Include(c => c.SecondPassnger).Include(c => c.ThirdPassnger).Include(c => c.FourthPassnger).FirstOrDefaultAsync(c => c.Id == model.TaxicabId);

            if (Service == null)
            {
                return Json(new { state = false, message = "سرویسی یافت نشد" });
            }
            else
            {
                // حذف فاکتور
                var driverfactor = await _context.driverFactors
                          .Where(c => c.taxiCabeid == model.TaxicabId &&
                          c.serviceRequsetId == model.Requsetserviceid).ToListAsync();
                _context.driverFactors.RemoveRange(driverfactor);

                if (Passenger.cabAsFirst != null)
                {
                    try
                    {

                        Passenger.cabAsFirst = null;
                        Passenger.RequsetState = RequsetSate.pending;
                        Service.IsCompelete = false;
                        _context.Update(Passenger);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {
                        await _logger.LogAsync(ex);
                        return Json(new { state = true, message = "خطایی بوجود آمد" });
                    }

                }
                else if (Passenger.cabAsSecond != null)
                {
                    try
                    {

                        Passenger.cabAsSecond = null;
                        Passenger.RequsetState = RequsetSate.pending;
                        Service.IsCompelete = false;
                        _context.Update(Passenger);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {

                        await _logger.LogAsync(ex);
                        return Json(new { state = true, message = "خطایی بوجود آمد" });
                    }

                }
                else if (Passenger.cabAsThird != null)
                {
                    try
                    {

                        Passenger.cabAsThird = null;
                        Passenger.RequsetState = RequsetSate.pending;
                        Service.IsCompelete = false;
                        _context.Update(Passenger);
                        _context.driverFactors.RemoveRange(driverfactor);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {

                        await _logger.LogAsync(ex);
                        return Json(new { state = true, message = "خطایی بوجود آمد" });
                    }

                }
                else if (Passenger.cabAsFourth != null)
                {
                    try
                    {

                        Passenger.cabAsFourth = null;
                        Passenger.RequsetState = RequsetSate.pending;
                        Service.IsCompelete = false;
                        _context.Update(Passenger);
                        _context.driverFactors.RemoveRange(driverfactor);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (Exception ex)
                    {

                        await _logger.LogAsync(ex);
                        return Json(new { state = true, message = "خطایی بوجود آمد" });
                    }

                }
                return Json(new { state = true, message = "مسافر از تاکسی خارج شد" });
            }
        }


        /// <summary>
        /// افزدون فاکتور بصورت ایجکسی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route(nameof(AddFactorAjax))]
        public async Task<IActionResult> AddFactorAjax([FromBody] DriverFactorviewModel model)
        {
            if (model == null)
                return Json(new { state = false, message = "مدلی یافت نشد" });
            try
            {
                var Engfrom = model.From.ToGregorianDateTime();
                var EngTo = model.To.ToGregorianDateTime();
                if (!Engfrom.HasValue)
                    return Json(new { state = false, message = "تاریخ شروع اشتباه وارد شده است" });
                if (!EngTo.HasValue)
                    return Json(new { state = false, message = "تاریخ پایان اشتباه وارد شده است" });
                if (Engfrom.Value >= EngTo.Value)
                    return Json(new { state = false, message = "تاریخ پایان نباید قبل یا برابر با تاریخ شروع باشد" });

                if (model.SeassionCount <= 0)
                    return Json(new { state = false, message = "تعداد جلسات اشتباه وارد شده است" });
                DriverFactor f = new DriverFactor()
                {
                    From = Engfrom,
                    To = EngTo,
                    SeassionCount = model.SeassionCount,
                    serviceRequsetId = model.Requsetserviceid,
                    taxiCabeid = model.Taxicabid,

                };

                await _context.AddAsync(f);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return Json(new { state = true, message = "فاکتور ثبت شد" });

            }
            catch (Exception ex)
            {
                await _logger.LogAsync(ex);
                return Json(new { state = false, message = "متاسفانه فاکتور ثبت نشد" });
            }



        }



        private bool TaxiCabExists(int id)
        {
            return _context.TaxiServices.Any(e => e.Id == id);
        }


        private void ChangeServiceState(TaxiService Service)
        {
            var count = Service.FirstPassnger.Count() + Service.SecondPassnger.Count() + Service.ThirdPassnger.Count() + Service.FourthPassnger.Count();
            if (count == 4)
                Service.IsCompelete = true;
        }

        public IActionResult GetLogs()
        {

            var data = _logger.ReadLogs();
            return Ok(data);
        }
    }

    public class PassengerViewModel
    {
        public int Requsetserviceid { get; set; }
        public int TaxicabId { get; set; }
    }

    public class DriverFactorviewModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public int SeassionCount { get; set; }


        public int Taxicabid { get; set; }

        public int Requsetserviceid { get; set; }

    }
}

