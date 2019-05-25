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
        private ISMS<List<SendResult>> _sms;

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

            return _context.studentParents.Any(c => c.PhoneNubmber == PhoneNumber);

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
                var parrent = _context.studentParents.FirstOrDefault(c => c.PhoneNubmber == model.PhoneNubmber);
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
                    await _context.studentParents.AddAsync(parrent);
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
            var parrent = await _context.studentParents.Undelited()
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
                    if (requset.AcademyId != model.AcademyId)
                    {
                        requset.AcademyId = model.AcademyId;
                        _context.Update(requset);
                    }
                    requset.price = CalcPrice(requset);
                    if (requset.price == 0)
                    {
                        return Ok(new ResultContract<int> { message = "در حال حاضر امکان ارایه سرویس برای آموزشگاه شما وجود ندارد", statuse = false, Data = 0 });

                    }
                    requset.RequsetCode = utils.UniqGenerate();
                    await _context.serviceRequsets.AddAsync(requset);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                    return Ok(new ResultContract<int> { statuse = true, Data = requset.Id });
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
                var Academy = requset.Academy;
                if (Academy == null) throw new Exception("Academy IS Null");

                decimal Distance = requset.Distance;
                var id = Academy.district.CityId;
                var pricing = _context.pricings.Where(c => c.AcademyId == Academy.Id).ToList();
                if (pricing == null) return 0;

                var priceModel = pricing.FirstOrDefault(c => c.FormKilometer <= Distance && c.ToKilometer > Distance);
                if (priceModel == null) throw new Exception("price IS Null");
                var generalSetting = _context.generalSettings.FirstOrDefault();
                int numberOfSeasion = 16;
                if (generalSetting != null && generalSetting.SeasionCount > 0)
                {
                    numberOfSeasion = generalSetting.SeasionCount;
                }

                double distancPerKilometer = (double)Math.Round(Distance) * priceModel.PricePerKilometer;
                double price = (priceModel.ConstPrice + distancPerKilometer);
                var commision = (price * priceModel.Comission);
                price = price + (commision / 100);
                double trafficPercent = 0;
                price = price + (trafficPercent / 100);
                price = price * (numberOfSeasion * 2);
                int fp = (int)price;
                fp = fp * 10/*Rial*/;
                return fp;
            }
            catch (Exception ex)
            {

                return 0;
            }


        }



        /// <summary>
        /// دریافت سرویس ها با توجه به وضعیت  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ServiceHistory")]
        public async Task<IActionResult> ServiceHistory([FromBody] GetServiceHistoryViewModel model)
        {
            try
            {
                var data = await _context.serviceRequsets

                    .Include(c => c.Academy)

                    .Where(c => c.StudentParrentId == model.ParrentId && c.RequsetState == model.RequsetSate)
                    .ToListAsync();
                return Ok(new ResultContract<List<ServiceRequset>> { statuse = true, Data = data });
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
        public async Task<IActionResult> ServiceDetail([FromBody] getDetailViewModel model)
        {
            try
            {
                var data = await _context.serviceRequsets
                    .Include(c => c.Academy)

                    .Include(c => c.cabAsFirst).ThenInclude(cf => cf.Driver)
                    .Include(c => c.cabAsSecond).ThenInclude(cs => cs.Driver)
                    .Include(c => c.cabAsThird).ThenInclude(ct => ct.Driver)
                    .Include(c => c.cabAsFourth).ThenInclude(cfo => cfo.Driver)


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
        /// پرداخت
        /// </summary>
        /// <param name="model"></param>
        /// <returns>بازگشتی در صورت تایید آدرس نهایی پرداخت است</returns>
        [HttpPost("pay")]
        public async Task<IActionResult> pay([FromBody] PayViewModel model)
        {
            var serviceRequsets = await _context.serviceRequsets.FindAsync(model.requsetId);
            if (serviceRequsets != null)
            {
                if (serviceRequsets.StudentParrentId != model.ParrentId)
                {
                    await _logger.LogAsync(HttpContext, new Exception("StudentParrentId Not match as RequsetService_ParrentId"));
                    return Ok(new ResultContract<string>() { statuse = false, message = "درخواست معتبر نیست" });
                }
                if (serviceRequsets.price <= 1000)
                {
                    await _logger.LogAsync(HttpContext, new Exception("Price IS Less than 1000"));
                    return Ok(new ResultContract<string>() { statuse = false, message = "درخواست معتبر نیست" });
                }
                string Amount = serviceRequsets.price.ToString();

                TranactionViewModel tr = new TranactionViewModel()
                {
                    Cost = Amount,
                    PaynameID = 5,
                    PayTypeID = 2,
                    Success = false,
                    TrackingCode = "8" /*ثابت برای تاکسی*/,
                    UserId = model.requsetId
                };
                var hashed = tr.HashedObject(GetType().GetProperties());
                if (hashed == "-1")
                {
                    await _logger.LogAsync(HttpContext, "هش کردن آبجکت با خطا مواجه شد");
                    return Ok(new ResultContract<string>() { statuse = false, message = "خطایی بوجود آمد" });

                }
                string RedirectUrl = $"http://core.tamam.ir/gateway/pay?HashedData={hashed}";
                return Ok(new ResultContract<string>() { statuse = true, Data = RedirectUrl });

            }
            else
            {
                await _logger.LogAsync(HttpContext, new Exception("Requset IS Null"));
                return Ok(new ResultContract<string>() { statuse = false, message = "مشکلی در سمت سرور بوجودامد" });
            }
        }


        /// <summary>
        /// پرداخت موفق
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Setpayment")]
        public async Task<IActionResult> Setpayment([FromBody] TranactionViewModel model)
        {

            try
            {
                var service = await _context.serviceRequsets.FindAsync(model.UserId);
                if (service == null)
                {
                    await _logger.LogAsync(HttpContext, "serviceRequsets Is NULL");
                    return Ok(new ResultContract<List<string>> { statuse = false, message = "خطایی بوجود آمد" });
                }
                service.RequsetState = RequsetSate.pending;
                var payment = new Payment()
                {
                    Ammount = service.price.ToString(),
                    ParrentId = service.StudentParrentId,
                    RequsetServiceId = service.Id,
                    Success = model.Success,
                    TrackingCode = model.TrackingCode
                };
                _context.Update(service);
                await _context.payments.AddAsync(payment);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return Ok(new ResultContract<string> { statuse = true });


            }
            catch (Exception ex)
            {

                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<string> { statuse = false, message = "خطایی بوجود آمد" });

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
                List<Academy> data = await _context.academies.Undelited().ToListAsync();
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
        public async Task<IActionResult> GetAcademy([FromBody] getDetailViewModel model)
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                Academy data = await _context.academies.Undelited().FirstOrDefaultAsync(c => c.Id == model.Id);
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
            var data = await _context.districts.Undelited().ToListAsync();
            if(data==null)
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
            var data = await _context.academyCategories.Undelited().ToListAsync();
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
            var data = await _context.academies.Undelited()
                .Where(c => c.AllowActivity == true
                && c.districtId == model.DistrcitId
                && c.AcademyCategoryId == model.AcademyCaregoryId)
                .ToListAsync();
            if (data == null)
                return Ok(new ResultContract<List<Academy>> { statuse = false, message = "یافت نشد" });
            return Ok(new ResultContract<List<Academy>> { statuse = true, Data = data });
        }


        #endregion













































    }
}
