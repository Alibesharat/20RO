using DAL;
using Kavenegar.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotifCore;
using Shared;
using Shared.Contracts;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WepApplication.Models;
using WepApplication.Util;

namespace WepApplication.Controllers
{
    public class HomeController : Controller
    {
        private ISMS<List<SendResult>> _sms;
        public HomeController(ISMS<List<SendResult>> sms)
        {
            _sms = sms;
        }



        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> Index()
        {

            var parrent = User.Getparrent();
            if (parrent == null)
                return Json(new ResultContract<int>() { statuse = false, message = "برای استفاده از نرم افزار ابتدا وارد شوید" });

        

            var academy = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<Academy>>>(new getDetailViewModel() { Id = parrent.Id }, Const.GetAcademies, ApiMethode.Post);
            if (academy != null)
                ViewData["academy"] = new SelectList(academy.Data, "Id", "Name");


            return View();
        }



        [HttpPost]
        [Route("RequsetService")]
        public async Task<IActionResult> RequsetService([FromBody] RequsetServiceViewModel model)
        {
            if (model != null)
            {
                var parrent = User.Getparrent();
                if (parrent == null)
                    return Json(new ResultContract<int>() { statuse = false, message = "برای ثبت درخواست باید ابتدا وارد شوید " });
                model.StudentParrentId = parrent.Id;
                model.Age = parrent.Age;
                model.FullName = parrent.FullName;
                model.gender = parrent.Gender;

                string Distination = "";
                ResultContract<Academy> res = await ConnectApi.
                    GetDataFromHttpClientAsync<ResultContract<Academy>>
                    (new getDetailViewModel() { Id = model.CourseId }, Const.GetAcademyByCourse, ApiMethode.Post);
                if (res != null && res.statuse == true)
                {
                    Distination = $"{res.Data.Longtude},{res.Data.latitude}";
                }
                string origin = $"{model.Longtude},{model.latitue}";
                var navigation = await utils.GetNavigation(origin, Distination);
                if (navigation != null && navigation.code == "Ok")
                {
                    var dis = navigation.routes.FirstOrDefault().distance;
                    model.Distance = (((decimal)dis) / 1000);
                }
                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<int>>
                          (model, Const.RequsertService, ApiMethode.Post);
                if (data == null)
                    return Json(new ResultContract<int>() { statuse = false, message = "یافت نشد" });
                return Json(data);
            }
            else
            {
                return Json(new ResultContract<int>() { statuse = false, message = "اطلاعات ارسالی معتبر نبود" });


            }


        }



        [Route("ServiceDetail/{id}")]
        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> ServiceDetail(int id)
        {
            var model = new getDetailViewModel() { Id = id };
            ResultContract<ServiceRequset> data = await ConnectApi
                .GetDataFromHttpClientAsync<ResultContract<ServiceRequset>>
                (model, Const.ServiceDetail, ApiMethode.Post);

            if (data == null || data.statuse == false) return NotFound();
            if (data.Data.StudentParrentId != User.Getparrent()?.Id) return NotFound();
            data.Data.Distination = $"{data.Data.Academy.Longtude},{data.Data.Academy.latitude}";
            data.Data.Origin = $"{data.Data.Longtude},{data.Data.latitue}";
            return View(data.Data);
        }


        [Route("VerfiedPay/{id}/{trackingCode}")]
        public async Task<IActionResult> VerfiedPay(int id, string trackingCode)
        {
            List<string> PhoneNumbers = new List<string>();
            PhoneNumbers.Add(User.Getparrent().PhoneNubmber);
            string message = $"پرداخت شما در سامانه ایلیکار با موفقیت انجام شد  کد پیگیری  :  {trackingCode}";
            ViewBag.message = message;
            _sms.phoneNumbers = PhoneNumbers;
            _sms.message = message;
            var (stause, errormessage, results) = await _sms.SendNotifyAsync();
            ViewBag.id = id;
            return View();
        }


        public async Task<IActionResult> GetRoute([FromBody] RouteViewModel model)
        {
            var navigation = await utils.GetNavigation(model.origin, model.Distination);
            if (navigation != null && navigation.code == "Ok")
            {
                //var dis = navigation.routes.FirstOrDefault().distance;
                var geometry = navigation.routes.FirstOrDefault().geometry;
                return Json(geometry);
            }
            return Json(null);
        }


        /// <summary>
        /// سرویس های فعال
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> Activeservice()
        {
            var Parrent = User.Getparrent();
            if (Parrent == null) return Unauthorized();

            GetServiceHistoryViewModel vm = new GetServiceHistoryViewModel()
            {
                ParrentId = Parrent.Id,
                RequsetSate = RequsetSate.Servicing
            };
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<ServiceRequset>>>(vm, Const.ServiceHistory, ApiMethode.Post);
            if (data != null)
            {
                return View(data.Data);
            }
            else
            {
                return View(new List<ServiceRequset>());
            }

        }



        /// <summary>
        /// در انتظار پرداخت  
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> AwaitingPayment()
        {
            var Parrent = User.Getparrent();
            if (Parrent == null) return Unauthorized();

            GetServiceHistoryViewModel vm = new GetServiceHistoryViewModel()
            {
                ParrentId = Parrent.Id,
                RequsetSate = RequsetSate.AwaitingPayment
            };
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<ServiceRequset>>>(vm, Const.ServiceHistory, ApiMethode.Post);
            if (data != null)
            {
                return View(data.Data);
            }
            else
            {
                return View(new List<ServiceRequset>());
            }

        }


        /// <summary>
        /// لغو شده ها    
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> Cancel()
        {
            var Parrent = User.Getparrent();
            if (Parrent == null) return Unauthorized();

            GetServiceHistoryViewModel vm = new GetServiceHistoryViewModel()
            {
                ParrentId = Parrent.Id,
                RequsetSate = RequsetSate.Cancel
            };
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<ServiceRequset>>>(vm, Const.ServiceHistory, ApiMethode.Post);
            if (data != null)
            {
                return View(data.Data);
            }
            else
            {
                return View(new List<ServiceRequset>());
            }

        }


        /// <summary>
        /// در دست بررسی      
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> pending()
        {
            var Parrent = User.Getparrent();
            if (Parrent == null) return Unauthorized();

            GetServiceHistoryViewModel vm = new GetServiceHistoryViewModel()
            {
                ParrentId = Parrent.Id,
                RequsetSate = RequsetSate.pending
            };
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<ServiceRequset>>>(vm, Const.ServiceHistory, ApiMethode.Post);
            if (data != null)
            {
                return View(data.Data);
            }
            else
            {
                return View(new List<ServiceRequset>());
            }

        }


        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> pay([FromBody]PayViewModel vm)
        {
            var Parrent = User.Getparrent();
            if (Parrent == null) return Unauthorized();
            vm.ParrentId = Parrent.Id;
            var data = await ConnectApi
                .GetDataFromHttpClientAsync<ResultContract<string>>
                (vm, Const.pay, ApiMethode.Post);
            if (data == null)
            {

                return Json(new ResultContract<int>() { statuse = false, message = "مشکلی بوجود آمد" });
            }
            else
            {
                return Json(data);
            }
        }





        [Route("terms")]
        public IActionResult Terms()
        {
            return View();
        }


        //[Route("help")]
        //public IActionResult help()
        //{
            
        //    return new VirtualFileResult("ilicarhelp.pdf", "application/x-msdownload") { FileDownloadName = "ilicarhelp.pdf" };

        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
