using AlphaCoreLogger;
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

    [Authorize(Roles = nameof(RolName.Contractor))]
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

            var data = _context.serviceRequsets.Undelited().Where(c => c.RequsetState == RequsetSate.pending).ToList();
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



        public async Task<IActionResult> Read()
        {


            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"data.xlsx";
            FileInfo file = new FileInfo(Path.Combine(@"D:\\", sFileName));
            List<Academy> lst = new List<Academy>();
            try
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    List<Academy> lstex = new List<Academy>();
                    for (int row = 2; row <= rowCount; row++)
                    {

                        var name = worksheet.Cells[row, 3].Value?.ToString();
                        var Address = worksheet.Cells[row, 5].Value?.ToString();
                        var tel = worksheet.Cells[row, 6].Value?.ToString();
                        var disttrict = worksheet.Cells[row, 7].Value?.ToString();
                        var category = worksheet.Cells[row, 8].Value?.ToString();
                        int def = 1;
                        int defu = 1;
                        int.TryParse(category, out def);
                        int.TryParse(disttrict, out defu);
                        //lst.Add(new Academy()
                        //{
                        //    AcademyCategoryId = def,
                        //    Name = name,
                        //    Address = Address,
                        //    OfficeNumber = tel,
                        //    districtId = defu,
                        //    ContractorId=4

                        //});
                        Academy ac=new Academy();

                        try
                        {
                            ac = new Academy()
                            {
                                AcademyCategoryId = def,
                                Name = name,
                                Address = Address,
                                OfficeNumber = tel,
                                districtId = defu,
                                ContractorId = 4
                            };
                            await _context.academies.AddAsync(ac);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            lst.Add(ac);
                        }
                       

                    }

                  
                    return Json(lst);

                }
            }
            catch (Exception ex)
            {
                return Json("Some error occured while importing." + ex.Message);
            }



        }


    }
}
