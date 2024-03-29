﻿using AlphaCoreLogger;
using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;


namespace Web.Controllers
{

    [Authorize(Roles = nameof(RolName.Admin))]
    public class HomeController : Controller
    {
        private readonly TaxiContext _context;
        private readonly ICoreLogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(TaxiContext context, ICoreLogger logger, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {

            return View();
        }


        public IActionResult Distributionmap()
        {

            var data = _context.ServiceRequsets.Undelited().Where(c => c.RequsetState == RequsetSate.Servicing).ToList();
            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            string ExceptionMessage = "";
            var exceptionHandlerPathFeature =
    HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                ExceptionMessage = "File error thrown";
            }
            if (exceptionHandlerPathFeature?.Path == "/index")
            {
                ExceptionMessage += " from home page";
            }
            CoreLog log = new CoreLog()
            {
                ErrorMessage = ExceptionMessage,
                DateTime = DateTime.Now,
                RequsetPath = exceptionHandlerPathFeature?.Path,
            };
            _logger.Log(log);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Logs()
        {
            var logs = await _logger.ReadLogsAsync();
            return Json(logs);
        }



     


    }
}
