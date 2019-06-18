using AutoHistoryCore;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers
{

   [Authorize(Roles = nameof(RolName.Admin))]
    public class StudentParentsController : Controller
    {
        private readonly TaxiContext _context;

        public StudentParentsController(TaxiContext context)
        {
            _context = context;
        }

        // GET: StudentParents
        public async Task<IActionResult> Index(int pageindex = 1, string searchterm = "")
        {
            var takeStep = 10;
            var SkipStep = (pageindex - 1) * takeStep;
            int count = 0;
            ViewBag.curent = pageindex;
            Dictionary<string, string> AllRouteData = new Dictionary<string, string>();

            var _studentParents = _context.StudentParents.Undelited().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                _studentParents = _studentParents.Where(c => c.Name.Contains(searchterm));
                ViewBag.searchterm = searchterm;
            }

            count = _studentParents.Count();
            ViewBag.AllRouteData = AllRouteData;
            _studentParents = _studentParents.Skip(SkipStep).Take(takeStep);
            ViewData["Count"] = count;
            ViewBag.pageCount = (count / takeStep) + 1;
            return View(await _studentParents.ToListAsync());
        }

        // GET: StudentParents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentParent = await _context.StudentParents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentParent == null)
            {
                return NotFound();
            }

            return View(studentParent);
        }

        // GET: StudentParents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentParents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Age,Gender,Password,PhoneNubmber,IsMobielVerifed,Email,IsEmailVerified,AvatarPath,BirthDay,AllowActivity")] StudentParent studentParent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentParent);
                await _context.SaveChangesWithHistoryAsync(HttpContext);
                return RedirectToAction(nameof(Index));
            }
            return View(studentParent);
        }

        // GET: StudentParents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentParent = await _context.StudentParents.FindAsync(id);
            if (studentParent == null)
            {
                return NotFound();
            }
            return View(studentParent);
        }

        // POST: StudentParents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Age,Gender,Password,PhoneNubmber,IsMobielVerifed,Email,IsEmailVerified,AvatarPath,BirthDay,AllowActivity")] StudentParent studentParent)
        {
            if (id != studentParent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentParent);
                    await _context.SaveChangesWithHistoryAsync(HttpContext);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentParentExists(studentParent.Id))
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
            return View(studentParent);
        }

        // GET: StudentParents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentParent = await _context.StudentParents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentParent == null)
            {
                return NotFound();
            }

            return View(studentParent);
        }

        // POST: StudentParents/remove/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentParent = await _context.StudentParents.FindAsync(id);
            _context.StudentParents.Remove(studentParent);
            await _context.SaveChangesWithHistoryAsync(HttpContext);
            return RedirectToAction(nameof(Index));
        }

        private bool StudentParentExists(int id)
        {
            return _context.StudentParents.Any(e => e.Id == id);
        }



        //public async Task<IActionResult> Add()
        //{
        //    //var Studentparetn = new StudentParent()
        //    //{
        //    //    AcademyId = 21,
        //    //    Age = 15,
        //    //    Gender = false,
        //    //    AllowActivity = true,
        //    //    Name = "دیانا",
        //    //    LastName = "نجفی",
        //    //    Password = "9624050707",
        //    //    PhoneNubmber = "09128894935",

        //    //};
        //    //await  _context.studentParents.AddAsync(Studentparetn);
        //    //await _context.SaveChangesWithHistoryAsync(HttpContext);
        //    //return Json("Ok");
        //}
    }
}
