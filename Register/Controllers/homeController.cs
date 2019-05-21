using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DNTPersianUtils.Core;

namespace Panel.Controllers
{

    public class homeController : Controller
    {
        private readonly TaxiContext _context;

        public homeController(TaxiContext context)
        {
            _context = context;
        }

        // GET: Students/Create
        public IActionResult Register()
        {

            ViewData["AcademyId"] = new SelectList(_context.academies.Undelited().Where(c => c.AllowActivity == true), "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Name,LastName,Gender,IrIdCod,StudentCode,Age,PhoneNubmber,AcademyId,Hs_Change,IsDeleted,AccesptTerms")] StudentParent student)
        {

            if (student.AccesptTerms == false)
            {
                ModelState.AddModelError(nameof(student.AccesptTerms), "پذیرفتن قوانین و مقررات  برای ثبت نام الزامی است");

            }
            if (!student.IrIdCod.IsValidIranianNationalCode())
            {
                ModelState.AddModelError(nameof(student.IrIdCod), "کد ملی وارد شده معتبر نیست");
            }
            if (!student.PhoneNubmber.IsValidIranianMobileNumber())
            {
                ModelState.AddModelError(nameof(student.PhoneNubmber), "شماره موبایل وارد شده معتبر نیست (انگلیسی تایپ شود)");

            }
            if (ModelState.IsValid)
            {
                student.PhoneNubmber = student.PhoneNubmber.ToEnglishNumbers();
                student.StudentCode = student.StudentCode.ToEnglishNumbers();
                student.IrIdCod = student.IrIdCod.ToEnglishNumbers();

                var exsit = await _context.studentParents.AnyAsync(
                    c => c.IrIdCod == student.IrIdCod
                || c.StudentCode == student.StudentCode);
                if (exsit == false)
                {

                    try
                    {
                        StudentParent studentparent = new StudentParent()
                        {
                            AllowActivity = true,
                            Name = student.Name,
                            LastName = student.LastName,
                            Hs_Change = student.Hs_Change,
                            IsDeleted = student.IsDeleted,
                            Password = student.IrIdCod,
                            PhoneNubmber = student.PhoneNubmber,
                            Gender = student.Gender,
                            Age = student.Age,
                            StudentCode = student.StudentCode,
                            AcademyId = student.AcademyId,
                            AccesptTerms = student.AccesptTerms
                        };
                        _context.Add(student);
                        _context.Add(studentparent);
                        await _context.SaveChangesWithHistoryAsync(HttpContext);
                    }
                    catch (System.Exception ex)
                    {

                        throw;
                    }

                    return RedirectToAction(nameof(result), new { state = true });
                }
                else
                {
                    ModelState.AddModelError("", "این مشخصات از قبل در سامانه وجود دارد");
                    ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", student.AcademyId);
                    return View(student);
                }

            }
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", student.AcademyId);
            return View(student);
        }


        public IActionResult result(bool state)
        {
            ViewBag.msg = state;
            return View();
        }
    }
}
