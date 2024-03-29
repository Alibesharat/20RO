﻿using DAL;
using DAL.Contracts;
using DAL.ViewModels;
using Kavenegar.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotifCore;
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

        public HomeController()
        {

        }



        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> Index()
        {

            var parrent = User.Getparrent();
            if (parrent == null)
                return Json(new ResultContract<int>() { statuse = false, message = "برای استفاده از نرم افزار ابتدا وارد شوید" });

            var District = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<District>>>(null, Const.GetDistrcits, ApiMethode.Post);
            var academyCategory = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<AcademyCategory>>>(null, Const.GetAcademyCategories, ApiMethode.Post);
            var academies = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<Academy>>>(new AcademyFiterViewModel { AcademyCaregoryId = 1, DistrcitId = 1 }, Const.GetFiltredAcademeis, ApiMethode.Post);

            if (District != null)
                ViewData["District"] = new SelectList(District.Data, "Id", "Name");
            if (academyCategory != null)
                ViewData["academyCategory"] = new SelectList(academyCategory.Data, "Id", "Name");
            if (academies != null)
                ViewData["academy"] = new SelectList(academies.Data, "Id", "Name");

            return View();
        }



        [HttpPost]
        [Route("RequsetService")]
        public async Task<IActionResult> RequsetService([FromBody] RequsetServiceViewModel model)
        {
            if (!ModelState.IsValid || model.AcademyId <= 0)
            {
                return Json(new ResultContract<int>() { statuse = false, message = "تکمیل همه اطلاعات الزامی است" });
            }
            if (model != null)
            {
                var parrent = User.Getparrent();
                if (parrent == null)
                    return Json(new ResultContract<int>() { statuse = false, message = "برای ثبت درخواست باید ابتدا وارد شوید " });
                model.StudentParrentId = parrent.Id;

                string Distination = "";
                ResultContract<Academy> res = await ConnectApi.
                    GetDataFromHttpClientAsync<ResultContract<Academy>>
                    (new GetDetailViewModel() { Id = model.AcademyId.ToString() }, Const.GetAcademy, ApiMethode.Post);
                if (res != null && res.statuse == true)
                {
                    Distination = $"{res.Data.Longtude},{res.Data.Latitude}";
                }
                string origin = $"{model.Longtude},{model.Latitue}";
                var navigation = await utils.GetNavigation(origin, Distination);
                if (navigation != null && navigation.code == "Ok")
                {
                    var dis = navigation.routes.FirstOrDefault().distance;
                    model.Distance = (((decimal)dis) / 1000);
                }
                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<string>>
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


        [HttpPost]
        [Route(nameof(CancelOrAccept))]
        public async Task<IActionResult> CancelOrAccept([FromBody] CancelAndAcceptRequsetViewModel model)
        {
            if (model != null)
            {
                var parrent = User.Getparrent();
                if (parrent == null)
                    return Unauthorized();
                ResultContract<int> res = await ConnectApi.
                 GetDataFromHttpClientAsync<ResultContract<int>>
                 (new CancelAndAcceptRequsetViewModel() { RequsetState = model.RequsetState, Token = parrent.Token, RequsetId = model.RequsetId }, Const.CancelAndAcceptRequset, ApiMethode.Post);

                if (res != null && res.statuse == true)
                {
                    return Json(new ResultContract<int>() { statuse = true, message = res.message });
                }
                else
                {
                    return Json(new ResultContract<int>() { statuse = false, message = "ارتباط با سرور برقرار نشد" });

                }
            }
            return Json(new ResultContract<int>() { statuse = false, message = "اطلاعات ارسالی معتبر نبود" });

        }





        [Route("ServiceDetail/{id}")]
        [Authorize(Roles = nameof(RolName.Parrent))]
        public async Task<IActionResult> ServiceDetail(string id)
        {
            var model = new GetDetailViewModel() { Id = id };
            ResultContract<ServiceRequset> data = await ConnectApi
                .GetDataFromHttpClientAsync<ResultContract<ServiceRequset>>
                (model, Const.ServiceDetail, ApiMethode.Post);

            if (data == null || data.statuse == false) return NotFound();
            if (data.Data.StudentParrentId != User.Getparrent()?.Id) return NotFound();
            data.Data.Distination = $"{data.Data.Academy.Longtude},{data.Data.Academy.Latitude}";
            data.Data.Origin = $"{data.Data.Longtude},{data.Data.Latitue}";
            return View(data.Data);
        }



        public async Task<IActionResult> GetRoute([FromBody] RouteViewModel model)
        {
            var navigation = await utils.GetNavigation(model.Origin, model.Distination);
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
        public async Task<IActionResult> Pending()
        {
            var Parrent = User.Getparrent();
            if (Parrent == null) return Unauthorized();

            GetServiceHistoryViewModel vm = new GetServiceHistoryViewModel()
            {
                ParrentId = Parrent.Id,
                RequsetSate = RequsetSate.AwaitingAcademy
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
        public async Task<IActionResult> Reservs()
        {
            var Parrent = User.Getparrent();
            if (Parrent == null) return Unauthorized();

            GetServiceHistoryViewModel vm = new GetServiceHistoryViewModel()
            {
                ParrentId = Parrent.Id,
                RequsetSate = RequsetSate.Reserve
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



        public async Task<IActionResult> GetAcademyFilter([FromBody] AcademyFiterViewModel model)
        {
            if (model != null)
            {
                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<List<Academy>>>(model, Const.GetFiltredAcademeis, ApiMethode.Post);
                //ViewData["academy"] = new SelectList(data.Data, "Id", "Name");
                return Json(new ResultContract<List<Academy>>() { statuse = true, message = "یافت نشد", Data = data.Data });

            }
            else
            {
                return Json(new ResultContract<List<Academy>>() { statuse = false, message = "یافت نشد" });
            }

        }


        [Route("terms")]
        public IActionResult Terms()
        {
            return View();
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
