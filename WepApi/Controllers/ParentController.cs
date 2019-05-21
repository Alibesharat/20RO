﻿using System;
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
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPost("IsExistStudentparrent")]
        public IActionResult IsExistStudentparrent([FromBody] string phoneNumber)
        {
            try
            {
                bool state = CheckStudentparrent(phoneNumber);
                return Ok(new ResultContract<bool>() { statuse = true, Data = state });
            }
            catch (Exception ex)
            {
                _logger.Log(HttpContext, ex);
                return Ok(new ResultContract<bool>() { statuse = true, message = "مشکلی بوجود آمد" });


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
                if (!model.Password.Equals(model.Repassword))
                {
                    return Ok(new ResultContract<StudentParent>() { statuse = false, Data = null, message = "رمز عبور و تکرار آن برابر نیستند" });

                }
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
        /// بازیابی رمز عبور
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]LoginStudentParrentViewModel model)
        {
            try
            {
                var user = await _context.studentParents.FirstOrDefaultAsync(c => c.PhoneNubmber == model.PhoneNubmber);
                if (user != null)
                {
                    List<string> numbers = new List<string>();
                    numbers.Add(user.PhoneNubmber);
                    _sms.phoneNumbers = numbers;
                    _sms.message = $"رمز ایلیکار شما   : {user.Password}";
                    await _sms.SendNotifyAsync();

                }
                return Ok(new ResultContract<bool>() { statuse = true, Data = true, message = "چناچه شماره موبایل شما در سامانه ثبت شده باشد ، راهنمای بازیابی رمز عبور برای شما ارسال می شود" });

            }
            catch (Exception ex)
            {
                _logger.Log(HttpContext, ex);
                return Ok(new ResultContract<bool>() { statuse = true, message = "مشکلی بوجود آمد" });
            }

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
                var studentparrent = await _context.studentParents.FirstOrDefaultAsync(c => c.Id == requset.StudentParrentId);
                if (studentparrent != null)
                {
                    if (studentparrent.AcademyId != model.AcademyId)
                    {
                        studentparrent.AcademyId = model.AcademyId;
                        _context.Update(studentparrent);
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
                var course = _context.Courses.Include(c => c.Academy).ThenInclude(c => c.district).FirstOrDefault(c => c.Id == requset.CourseId);
                var Academy = course.Academy;
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
                double trafficPercent = price * course.trafficPercent;
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

                    .Include(c => c.course)
                    .ThenInclude(course => course.Academy)
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
                    .Include(c => c.course)
                    .ThenInclude(course => course.Academy)
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
        /// دریافت یک آموزشگاه 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAcademyByCourse")]
        public async Task<IActionResult> GetAcademyByCourse([FromBody] getDetailViewModel model)
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                Course data = await _context.Courses.Undelited().Include(c => c.Academy).FirstOrDefaultAsync(c => c.Id == model.Id);
                var ac = data.Academy;
                if (data == null || data.Academy == null)
                    return Ok(new ResultContract<Academy> { statuse = false, message = "یافت نشد" });

                return Ok(new ResultContract<Academy> { statuse = true, Data = ac });


            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<Academy> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }






        /// <summary>
        /// دریافت دوره ها
        /// </summary>
        /// <returns></returns>

        [HttpPost("GetCourses")]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                List<Course> data = await _context.Courses.Undelited().ToListAsync();
                return Ok(new ResultContract<List<Course>> { statuse = true, Data = data });


            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<List<Course>> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }

        /// <summary>
        /// دریافت دوره های یک آموزشگاه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetCourseByAcademy")]
        public async Task<IActionResult> GetCourseByAcademy([FromBody]getDetailViewModel model)
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                List<Course> data = await _context.Courses.Undelited().Where(c => c.AcademyId == model.Id).ToListAsync();
                return Ok(new ResultContract<List<Course>> { statuse = true, Data = data });


            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<List<Course>> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }



        /// <summary>
        /// دریافت  یک دوره
        /// </summary>
        /// <returns></returns>

        [HttpPost("GetCourse")]
        public async Task<IActionResult> GetCourse([FromBody]getDetailViewModel model)
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                Course data = await _context.Courses.Undelited().FirstOrDefaultAsync(c => c.Id == model.Id);
                if (data == null)
                    return Ok(new ResultContract<Course> { statuse = false, message = "یافت نشد" });
                return Ok(new ResultContract<Course> { statuse = true, Data = data });


            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<Course> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }
        #endregion


        /// <summary>
        /// دریافت دوره های  آموزشگاهی که کاربر ثبت نام کرده است
        /// </summary>
        /// <returns></returns>

        [HttpPost("GetCoursesByUser")]
        public async Task<IActionResult> GetCoursesByUser([FromBody]getDetailViewModel model)
        {
            try
            {
                _context.ChangeTracker.LazyLoadingEnabled = false;
                var studentParent = await _context.studentParents.Undelited().Include(c => c.academy).ThenInclude(ac => ac.Courses).FirstOrDefaultAsync(c => c.Id == model.Id);
                ICollection<Course> data = null;
                if (studentParent != null && studentParent.academy.Courses != null)
                {
                    data = studentParent.academy.Courses;
                }
                //List<Course> data = await _context.Courses.Undelited().Include(c=>c.Academy).ThenInclude(ac=>ac.StudentParents).Where(c=>c.Academy.stu.Id==Userid).ToListAsync();
                return Ok(new ResultContract<ICollection<Course>> { statuse = true, Data = data });


            }
            catch (Exception ex)
            {
                await _logger.LogAsync(HttpContext, ex);
                return Ok(new ResultContract<List<Course>> { statuse = false, message = "خطایی بوجود آمد" });

            }
        }









































    }
}
