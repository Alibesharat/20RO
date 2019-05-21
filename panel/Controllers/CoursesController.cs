using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoHistoryCore;
using DAL;
using NotifCore;
using Kavenegar.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Panel.Controllers
{

   [Authorize(Roles = nameof(RolName.Admin))]
    public class CoursesController : Controller
    {
        private readonly TaxiContext _context;
        private ISMS<List<SendResult>> _sms;

        public CoursesController(TaxiContext context, ISMS<List<SendResult>> sms)
        {
            _context = context;
            _sms = sms;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int? AcademyId, int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();
            var _Courses = _context.Courses.Undelited().AsQueryable();
            _Courses = _Courses.Include(c => c.Academy);
          
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _Courses = _Courses.Where(c => c.TeacherName.Contains(searchterm));
                count = _Courses.Count();
                ViewBag.searchterm = searchterm;
               
            }
            if (AcademyId.HasValue)
            {
                _Courses = _Courses.Where(c => c.AcademyId == AcademyId);
                AllRouteData.Add(nameof(AcademyId), AcademyId.ToString());
            }
            count = _Courses.Count();
            _Courses = _Courses.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            ViewBag.AllRouteData = AllRouteData;
            if (TempData["msg"] != null)
            {
                ViewBag.msg = TempData["msg"];
            }
            return View(await _Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Academy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create(int? Academyid)
        {

            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", Academyid);
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BeginDate,trafficPercent,EndDateTime,TeacherName,AcademyId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", course.AcademyId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", course.AcademyId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BeginDate,trafficPercent,EndDateTime,TeacherName,AcademyId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AcademyId"] = new SelectList(_context.academies, "Id", "Name", course.AcademyId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Academy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> CancelReport(int? CourseId)
        {
            if (!CourseId.HasValue)
                return NotFound();
            var Course = await _context.Courses.Include(c => c.ServiceRequsets).ThenInclude(ServiceRequsets => ServiceRequsets.studentParent).FirstOrDefaultAsync(c => c.Id == CourseId.Value);
            if (Course == null)
            {
                return NotFound();
            }
            var reqs = Course.ServiceRequsets.Where(c => c.RequsetState == RequsetSate.Servicing).ToList();
            List<string> PhoneNumbers = new List<string>();
            foreach (var item in reqs)
            {
                PhoneNumbers.Add(item.studentParent?.PhoneNubmber);
            }
           
            string message = $"به اطلاع می رساند کلاس {Course.Name}  امروز تعطیل است";
            _sms.phoneNumbers = PhoneNumbers;
            _sms.message = message;
            var (stause, errormessage, results) = await _sms.SendNotifyAsync();
            if (stause == true)
            {
                //DO Something like save in database
                TempData["msg"] = results.FirstOrDefault()?.Message;
            }
            else
            {
                TempData["msg"] = errormessage;

            }
            return RedirectToAction(nameof(Index), new { Course.AcademyId });

        }

      

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }


    }
}
