using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5_TH.Data;
using Lab5_TH.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5_TH.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext _context;

        public LearnerController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? mid)
        {
            ViewBag.Majors = new SelectList(_context.Majors, "MajorID", "MajorName");

            IQueryable<Learner> learnersQuery = _context.Learners
                .Include(l => l.Major)
                .Include(l => l.Enrollments)
                    .ThenInclude(e => e.Course);

            if (mid.HasValue && mid > 0)
            {
                learnersQuery = learnersQuery.Where(l => l.MajorID == mid.Value);
                ViewBag.SelectedMajor = mid.Value;
            }

            var learners = await learnersQuery.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_LearnerTable", learners);
            }

            return View(learners);
        }

        [HttpGet]
        public async Task<IActionResult> LearnerByMajorID(int mid)
        {
            var learners = await _context.Learners
                .Include(l => l.Major)
                .Include(l => l.Enrollments)
                    .ThenInclude(e => e.Course)
                .Where(l => l.MajorID == mid)
                .ToListAsync();

            return PartialView("_LearnerTable", learners);
        }

        [HttpGet]
        public async Task<IActionResult> AllLearners()
        {
            var learners = await _context.Learners
                .Include(l => l.Major)
                .Include(l => l.Enrollments)
                    .ThenInclude(e => e.Course)
                .ToListAsync();

            return PartialView("_LearnerTable", learners);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.Major)
                .Include(l => l.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(m => m.LearnerID == id);

            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        public IActionResult Create()
        {
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerID,LastName,FirstMidName,EnrollmentDate,MajorID")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners.FindAsync(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerID,LastName,FirstMidName,EnrollmentDate,MajorID")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
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
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.Major)
                .Include(l => l.Enrollments)
                .FirstOrDefaultAsync(m => m.LearnerID == id);

            if (learner == null)
            {
                return NotFound();
            }

            if (learner.Enrollments.Any())
            {
                ViewBag.ErrorMessage = "This learner has enrollments and cannot be deleted.";
                return View(learner);
            }

            return View(learner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learner = await _context.Learners.FindAsync(id);
            if (learner != null)
            {
                var hasEnrollments = await _context.Enrollments.AnyAsync(e => e.LearnerID == id);
                if (hasEnrollments)
                {
                    TempData["ErrorMessage"] = "Cannot delete learner with existing enrollments.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }

                _context.Learners.Remove(learner);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(int id)
        {
            return _context.Learners.Any(e => e.LearnerID == id);
        }
    }
}