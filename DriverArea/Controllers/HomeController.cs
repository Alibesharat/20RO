using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
using Shared.Contracts;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DriverArea.Models;
using DriverArea.Util;
using Newtonsoft.Json;

namespace DriverArea.Controllers
{
    public class HomeController : Controller
    {

        [Route("ServiceDetail/{id}")]
        public async Task<IActionResult> ServiceDetail(int id)
        {

            var Driver = User.GetDriver();
            if (Driver == null)
            {
                return View(new TaxiService());
            }
            var model = new GetTaxiCabDetailViewModel() { TaxiCabId = id, DriverId = Driver.Id };
            ResultContract<string> data = await ConnectApi
                .GetDataFromHttpClientAsync<ResultContract<string>>
                (model, Const.GetTaxiCabDetail, ApiMethode.Post);
            //var setting = new JsonSerializerSettings
            //{
            //    PreserveReferencesHandling = PreserveReferencesHandling.Objects
            //};
            var dt = JsonConvert.DeserializeObject<TaxiService>(data.Data);
            if (dt == null) return View(new TaxiService());
            return View(dt);
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

        [Authorize(Roles = nameof(RolName.Driver))]
        public async Task<IActionResult> Activeservice()
        {
            var driver = User.GetDriver();
            if (driver == null) return Unauthorized();
            GetTaxiCabHistoryViewModel vm = new GetTaxiCabHistoryViewModel()
            {
                DriverId = driver.Id,
                TaxiCabState = TaxiCabState.Ready
            };
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<string>>(vm, Const.GetTaxiCabHistory, ApiMethode.Post);
            var dt = JsonConvert.DeserializeObject<List<TaxiService>>(data.Data);
            if (dt != null)
            {
                return View(dt);
            }
            else
            {
                return View(new List<TaxiService>());
            }

        }



        /// <summary>
        /// سرویس های در انتظار تایید
        /// </summary>
        /// <returns></returns>

        [Authorize(Roles = nameof(RolName.Driver))]
        public async Task<IActionResult> WaitngService()
        {
            var driver = User.GetDriver();
            if (driver == null) return Unauthorized();
            GetTaxiCabHistoryViewModel vm = new GetTaxiCabHistoryViewModel()
            {
                DriverId = driver.Id,
                TaxiCabState = TaxiCabState.WaitForDriverAccept
            };
            var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<string>>(vm, Const.GetTaxiCabHistory, ApiMethode.Post);
            var dt = JsonConvert.DeserializeObject<List<TaxiService>>(data.Data);
            if (dt != null)
            {
                return View(dt);
            }
            else
            {
                return View(new List<TaxiService>());
            }

        }







        [HttpPost]
        [Route(nameof(ChangeState))]
        public async Task<IActionResult> ChangeState([FromBody]ChangestateViewModel model)
        {
            var driver = User.GetDriver();
            if (driver != null)
            {
                model.DriverId = driver.Id;
                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<bool>>(model, Const.ChangeState, ApiMethode.Post);
                if (data != null)
                {

                    return Json(data);
                }
                else
                {
                    return Json(new ResultContract<bool>() { statuse = false, message = Const.InterntErrorMessag });
                }
            }
            return Json(new ResultContract<bool>() { statuse = false, message = Const.PremisionErrorMessag });


        }


        [HttpPost]
        [Route(nameof(AcceptTaxiCab))]
        public async Task<IActionResult> AcceptTaxiCab([FromBody]AccesptDriverViewModel model)
        {
            var driver = User.GetDriver();
            if (driver != null)
            {
                model.DriverId = driver.Id;
                var data = await ConnectApi.GetDataFromHttpClientAsync<ResultContract<bool>>(model, Const.AcceptDriver, ApiMethode.Post);
                if (data != null)
                {

                    return Json(data);
                }
                else
                {
                    return Json(new ResultContract<bool>() { statuse = false, message = Const.InterntErrorMessag });
                }
            }
            return Json(new ResultContract<bool>() { statuse = false, message = Const.PremisionErrorMessag });


        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
