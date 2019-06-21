using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaCoreLogger;
using AutoHistoryCore;
using DAL;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.ViewModels;
using Shared;
using NotifCore;
using Kavenegar.Core.Models;

namespace WepApi.Controllers
{
    /// <summary>
    /// ParentController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly TaxiContext _context;
        private readonly ICoreLogger _logger;
        private readonly ISMS<List<SendResult>> _sms;

        public ParentController(TaxiContext context, ICoreLogger logger, ISMS<List<SendResult>> sms)
        {
            _context = context;
            _logger = logger;
            _sms = sms;
        }




        #region والدین


        /// <summary>
        /// ایا والدین وجود دارد؟
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        private bool CheckStudentparrent(string PhoneNumber)
        {

            return _context.StudentParents.Any(c => c.PhoneNubmber == PhoneNumber);

        }


        /// <summary>
        /// آیا والدین از قبل ثبت نام کرده است ؟
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(IsExistStudentparrent))]
        public IActionResult IsExistStudentparrent([FromBody] RegisterStudentParrentViewModel model)
        {
            try
            {
                var parrent = _context.StudentParents.FirstOrDefault(c => c.PhoneNubmber == model.PhoneNubmber);
                if (parrent != null)
                {
                    return Ok(new ResultContract<StudentParent>() { statuse = true, Data = parrent });
                }
                return Ok(new ResultContract<StudentParent>() { statuse = false, Data = null, message = "این شماره موبایل از قبل ثبت نام کرده است" });

            }
            catch (Exception ex)
            {
                _logger.Log(HttpContext, ex);
                return Ok(new ResultContract<bool>() { statuse = false, message = "مشکلی بوجود آمد" });


            }



        }


        /// <summary>
        /// ثبت نام والدین
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("RegisterStudentParent")]
        public async Task<IActionResult> RegisterStudentParent([FromBody] RegisterStudentParrentViewModel model)
        {
            try
            {
                if (CheckStudentparrent(model.PhoneNubmber))
                {
                    return Ok(new ResultContract<StudentParent>() { statuse = false, Data = null, message = "این شماره موبایل قبلا ثبت نام کرده است" });
                }
                else
                {
                    var parrent = model.Adapt<StudentParent>();
                    await _context.StudentParents.AddAsync(parrent);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                    return Ok(new ResultContract<StudentParent>() { statuse = true, Data = parrent, message = "" });
                }

            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<StudentParent>() { statuse = false, Data = null, message = "مشکلی بوجود  آمد" });

            }
        }


        /// <summary>
        /// ورود والدین
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("LoginStudentParrent")]
        public async Task<IActionResult> LoginStudentParrent([FromBody]LoginStudentParrentViewModel model)
        {
            var parrent = await _context.StudentParents.Undelited()
                .FirstOrDefaultAsync(c => c.PhoneNubmber == model.PhoneNubmber
                 && c.Password == model.Password);

            if (parrent == null)
                return Ok(new ResultContract<StudentParent>() { statuse = false, Data = null, message = "نام کاربری یا رمز عبور اشتباه است" });
            return Ok(new ResultContract<StudentParent>() { statuse = true, Data = parrent, });


        }



        /// <summary>
        /// درخواست سرویس
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("RequsertService")]
        public async Task<IActionResult> RequsertService([FromBody] RequsetServiceViewModel model)
        {
            try
            {


                var requset = model.Adapt<ServiceRequset>();
                if (requset != null)
                {
                    requset.Price = CalcPrice(requset);
                    if (requset.Price == 0)
                    {
                        return Ok(new ResultContract<string> { message = "در حال حاضر امکان ارایه سرویس برای آموزشگاه شما وجود ندارد", statuse = false, Data = null });

                    }
                    await _context.ServiceRequsets.AddAsync(requset);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                    return Ok(new ResultContract<string> { statuse = true, Data = requset.Id });
                }
                else
                {
                    return Ok(new ResultContract<int> { message = "عملیات غیر مجاز است", statuse = false, Data = 0 });

                }

            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<int> { message = "خطایی در سمت سرور بوجود آمد", statuse = false, Data = 0 });
            }


        }

        /// <summary>
        /// محاسبه قیمت
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        private int CalcPrice(ServiceRequset requset)
        {
            try
            {
                var Academy = _context.Academies
                    .Undelited()
                    .Include(c => c.District)
                    .ThenInclude(c => c.City)
                    .FirstOrDefault(c => c.Id == requset.AcademyId && c.AllowActivity == true);
                if (Academy == null) return 0;

                int DistrictPercent = Academy.District.DistrictPercent;
                int CityPercent = Academy.District.City.CityPercent;
                int AcademyPercent = Academy.AcademyPercent;
                int ServiceTypePercent = 0;
                var generalSetting = _context.GeneralSettings.FirstOrDefault();
                if (generalSetting != null)
                {
                    if (requset.ServiceType == ServiceType.taxi)
                    {
                        ServiceTypePercent = generalSetting.TaxiPercent;
                    }
                    else
                    {
                        ServiceTypePercent = generalSetting.VanPercent;

                    }
                }

                int price = ServiceTypePercent * AcademyPercent * DistrictPercent * CityPercent;



                return price /*Rial*/;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return 0;
            }


        }



        /// <summary>
        /// دریافت سرویس ها با توجه به وضعیت  
        /// </summary>
        /// <param name="model"></param>
        /// <returns>مقدار بازگشتی برای حالات آموزشگاه و  پیمانکار بیکسان باشد</returns>
        [HttpPost("ServiceHistory")]
        public async Task<IActionResult> ServiceHistory([FromBody] GetServiceHistoryViewModel model)
        {
            try
            {
                if (model.RequsetSate == RequsetSate.AwaitingAcademy)
                {
                    var data = await _context.ServiceRequsets
                  .Undelited()
                  .Include(c => c.Academy)
                  .Where(c => c.StudentParrentId == model.ParrentId &&
                  (c.RequsetState == RequsetSate.AwaitingAcademy
                  || c.RequsetState == RequsetSate.AwaitingContractor))
                  .ToListAsync();
                    return Ok(new ResultContract<List<ServiceRequset>> { statuse = true, Data = data });
                }
                else
                {
                    var data = await _context.ServiceRequsets
                  .Undelited()
                  .Include(c => c.Academy)
                  .Where(c => c.StudentParrentId == model.ParrentId && c.RequsetState == model.RequsetSate)
                  .ToListAsync();
                    return Ok(new ResultContract<List<ServiceRequset>> { statuse = true, Data = data });
                }

            }
            catch (Exception ex)
            {

                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<List<ServiceRequset>> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }


        /// <summary>
        /// دریافت جزییات سرویس  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ServiceDetail")]
        public async Task<IActionResult> ServiceDetail([FromBody] GetDetailViewModel model)
        {
            try
            {
                var data = await _context.ServiceRequsets
                    .Include(c => c.Academy)
                    .Include(c => c.TaxiService)
                    .ThenInclude(t => t.Driver)
                    .FirstOrDefaultAsync(c => c.Id == model.Id);
                return Ok(new ResultContract<ServiceRequset> { statuse = true, Data = data });
            }
            catch (Exception ex)
            {

                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<ServiceRequset> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }








        /// <summary>
        /// دریافت آموزشگاه ها
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAcademies")]
        public async Task<IActionResult> GetAcademies()
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                List<Academy> data = await _context.Academies.Undelited().ToListAsync();
                return Ok(new ResultContract<List<Academy>> { statuse = true, Data = data });


            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<List<Academy>> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }



        /// <summary>
        /// دریافت یک آموزشگاه 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAcademy")]
        public async Task<IActionResult> GetAcademy([FromBody] GetDetailViewModel model)
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                Academy data = await _context.Academies.Undelited().FirstOrDefaultAsync(c => c.Id == int.Parse(model.Id));
                if (data == null)
                    return Ok(new ResultContract<Academy> { statuse = false, message = "یافت نشد" });

                return Ok(new ResultContract<Academy> { statuse = true, Data = data });


            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<Academy> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }

        /// <summary>
        /// دریافت لیست   مناطق
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(GetDistrcits))]
        public async Task<IActionResult> GetDistrcits()
        {
            var data = await _context.Districts.Undelited().ToListAsync();
            if (data == null)
                return Ok(new ResultContract<List<District>> { statuse = false, message = "یافت نشد" });
            return Ok(new ResultContract<List<District>> { statuse = true, Data = data });


        }


        /// <summary>
        /// دریافت لیست  گروه های آموزشگاه
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(GetAcademyCategories))]
        public async Task<IActionResult> GetAcademyCategories()
        {
            var data = await _context.AcademyCategories.Undelited().ToListAsync();
            if (data == null)
                return Ok(new ResultContract<List<AcademyCategory>> { statuse = false, message = "یافت نشد" });
            return Ok(new ResultContract<List<AcademyCategory>> { statuse = true, Data = data });

        }



        /// <summary>
        /// دریافت لیست  فیلتر شده آموزشگاه ها بر اساس مقطع و منطقه 
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(GetFiltredAcademeis))]
        public async Task<IActionResult> GetFiltredAcademeis(AcademyFiterViewModel model)
        {
            var data = await _context.Academies.Undelited()
                .Where(c => c.AllowActivity == true
                && c.DistrictId == model.DistrcitId
                && c.AcademyCategoryId == model.AcademyCaregoryId)
                .ToListAsync();
            if (data == null)
                return Ok(new ResultContract<List<Academy>> { statuse = false, message = "یافت نشد" });
            return Ok(new ResultContract<List<Academy>> { statuse = true, Data = data });
        }


        #endregion













































    }
}
