using AlphaCoreLogger;
using AutoHistoryCore;
using DAL;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Contracts;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WepApi.Controllers
{
    /// <summary>
    /// DriverController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : Controller
    {
        private readonly TaxiContext _context;
        private readonly ICoreLogger _logger;

        public DriverController(TaxiContext context, ICoreLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #region راننده
        /// <summary>
        /// ایا راننده وجود دارد؟
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        private bool CheckExistdriver(string PhoneNumber)
        {
            return _context.drivers.Any(c => c.PhoneNubmber == PhoneNumber);

        }


        /// <summary>
        /// آیا راننده از قبل ثبت نام کرده است ؟
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPost("IsExistdriver")]
        public IActionResult IsExistdriver([FromBody] string phoneNumber)
        {
            try
            {
                bool state = CheckExistdriver(phoneNumber);
                return Ok(new ResultContract<bool>() { statuse = true, Data = state });
            }
            catch (Exception ex)
            {
                _logger.Log(HttpContext, ex);
                return Ok(new ResultContract<string>() { statuse = false, message = "مشکلی بوجود آمد" });



            }



        }


        /// <summary>
        /// ثبت نام راننده
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Registerdriver")]
        public async Task<IActionResult> Registerdriver([FromBody] RegisterDriverViewModel model)
        {
            try
            {
                if (CheckExistdriver(model.PhoneNubmber))
                {
                    return Ok(new ResultContract<Driver>() { statuse = false, Data = null, message = "این شماره موبایل قبلا ثبت نام کرده است" });
                }
                else
                {
                    var driver = model.Adapt<Driver>();
                    if (driver.CityId == 0)
                        driver.CityId = 1;
                    await _context.drivers.AddAsync(driver);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                    return Ok(new ResultContract<Driver>() { statuse = true, Data = driver, message = "" });
                }

            }
            catch (Exception ex)
            {

                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<string>() { statuse = false, message = "یک خطای ناشناخته روی داد" });

            }
        }



        /// <summary>
        /// ورود  راننده
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Logindriver")]
        public async Task<IActionResult> Logindriver([FromBody] LoginDriverViewModel model)
        {
            try
            {
                var driver = await _context.drivers.Undelited().FirstOrDefaultAsync(
                    c => c.PhoneNubmber == model.PhoneNubmber && c.Password == model.Password);
                if (driver == null)
                    return Ok(new ResultContract<Driver>() { statuse = false, message = "رمز عبور یا نام کاربری اشتباه است" });
                return Ok(new ResultContract<Driver>() { statuse = true, Data = driver });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<Driver>() { statuse = false, message = "یک خطای ناشناخته روی داد" });

            }
        }


        /// <summary>
        /// تغییر وضعیت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ChangeState")]
        public async Task<IActionResult> ChangeState([FromBody] changestateViewModel model)
        {
            try
            {
                #region بررسی اعتبار درخواست کننده
                var taxtiCab = await _context.taxiCabs.Undelited().FirstOrDefaultAsync(c => c.Id == model.taxiCabId);
                if (taxtiCab == null || taxtiCab.DriverId != model.driverId)
                {
                    await _logger.LogAsync(HttpContext, $"{nameof(taxtiCab)} Is NULL");
                    return Ok(new ResultContract<bool>() { statuse = false, Data = false, message = "مشکلی بوجود آمد" });

                }
                #endregion

                var Service = await _context.serviceRequsets.Include(c => c.cabAsFirst).FirstOrDefaultAsync(c => c.Id == model.requseteId);

                if (Service == null)
                {
                    await _logger.LogAsync(HttpContext, $"{nameof(Service)} Is NULL");
                    return Ok(new ResultContract<bool>() { statuse = false, Data = false, message = "مشکلی بوجود آمد" });
                }

                Service.NotifState = model.notifState;
                _context.Update(Service);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return Ok(new ResultContract<bool>() { statuse = true, Data = true });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<bool>() { statuse = false, Data = false, message = "مشکلی بوجود آمد" });

            }

        }

        /// <summary>
        /// تایید سرویس توسط راننده
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(AcceptService))]
        public async Task<IActionResult> AcceptService([FromBody] AccesptDriverViewModel model)
        {
            try
            {
                #region بررسی اعتبار درخواست کننده
                var taxtiCab = await _context.taxiCabs.Undelited().FirstOrDefaultAsync(c => c.Id == model.taxiCabId);
                if (taxtiCab == null || taxtiCab.DriverId != model.driverId)
                {
                    await _logger.LogAsync(HttpContext, $"{nameof(taxtiCab)} Is NULL");
                    return Ok(new ResultContract<bool>() { statuse = false, Data = false, message = "مشکلی بوجود آمد" });

                }
                #endregion


                taxtiCab.TaxiCabState = TaxiCabState.DriverAccept;
                _context.Update(taxtiCab);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return Ok(new ResultContract<bool>() { statuse = true, Data = true });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<bool>() { statuse = false, Data = false, message = "مشکلی بوجود آمد" });

            }

        }



        /// <summary>
        ///  مشاهده سوابق تاکسی سرویس ها
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetTaxiCabHistory))]
        public async Task<IActionResult> GetTaxiCabHistory([FromBody] GetTaxiCabHistoryViewModel model)
        {
            try
            {
                var TaxiCabs = _context.taxiCabs.Include(c=>c.FirstPassnger).Where(c => c.DriverId == model.DriverId && c.TaxiCabState == model.TaxiCabState).ToList();
                var setting = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
                var data = JsonConvert.SerializeObject(TaxiCabs, setting);
                return Ok(new ResultContract<string>() { statuse = true, Data = data });

            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<List<TaxiCab>>() { statuse = false, message = "مشکلی بوجود آمد" });

            }

        }



        /// <summary>
        ///  مشاهده جزییات یک تاکسی سرویس 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetTaxiCabDetail")]
        public async Task<IActionResult> GetTaxiCabDetail([FromBody] GetTaxiCabDetailViewModel model)
        {
            try
            {
                var TaxiCab = await _context.taxiCabs
               .Include(c => c.FirstPassnger)
                .ThenInclude(c => c.Academy)
               
           

               .Include(c => c.SecondPassnger)
                  .ThenInclude(cst => cst.Academy)
              

               .Include(c => c.ThirdPassnger)
                 .ThenInclude(cts => cts.Academy)
             

               .Include(c => c.FourthPassnger)
                     .ThenInclude(cfs => cfs.Academy)
              

               .FirstOrDefaultAsync(c => c.Id == model.TaxiCabId && c.DriverId == model.DriverId);


                var setting = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
                var data = JsonConvert.SerializeObject(TaxiCab, setting);
                return Ok(new ResultContract<string>() { statuse = true, Data = data });

            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<List<TaxiCab>>() { statuse = false, message = "مشکلی بوجود آمد" });

            }

        }




        #endregion

    }
}