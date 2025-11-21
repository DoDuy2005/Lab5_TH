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
        private readonly int _pageSize = 3; // Số phần tử trên 1 trang

        public LearnerController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Learner
        public IActionResult Index(int? mid, string? keyword, int? pageIndex)
        {
            var learners = GetFilteredLearners(mid, keyword);

            // Lấy chỉ số trang
            int page = pageIndex ?? 1;
            if (page <= 0) page = 1;

            // Tính số trang
            int totalCount = learners.Count();
            int pageNum = (int)Math.Ceiling(totalCount / (float)_pageSize);
            ViewBag.pageNum = pageNum;
            ViewBag.mid = mid;
            ViewBag.keyword = keyword;
            ViewBag.currentPage = page;

            // Lấy dữ liệu trang hiện tại
            var result = learners.Skip(_pageSize * (page - 1))
                                .Take(_pageSize)
                                .Include(m => m.Major)
                                .ToList();

            return View(result);
        }

        // GET: Learner/LearnerFilter - Action cho AJAX
        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            var learners = GetFilteredLearners(mid, keyword);

            // Lấy chỉ số trang
            int page = pageIndex ?? 1;
            if (page <= 0) page = 1;

            // Tính số trang
            int totalCount = learners.Count();
            int pageNum = (int)Math.Ceiling(totalCount / (float)_pageSize);
            ViewBag.pageNum = pageNum;
            ViewBag.mid = mid;
            ViewBag.keyword = keyword;
            ViewBag.currentPage = page;

            // Lấy dữ liệu trang hiện tại
            var result = learners.Skip(_pageSize * (page - 1))
                                .Take(_pageSize)
                                .Include(m => m.Major)
                                .ToList();

            return PartialView("_LearnerTable", result);
        }

        private IQueryable<Learner> GetFilteredLearners(int? mid, string? keyword)
        {
            var learners = _context.Learners.AsQueryable();

            // Lọc theo major nếu có
            if (mid != null && mid > 0)
            {
                learners = learners.Where(l => l.MajorID == mid);
            }

            // Tìm kiếm theo keyword nếu có
            if (!string.IsNullOrEmpty(keyword))
            {
                learners = learners.Where(l => l.FirstMidName.ToLower().Contains(keyword.ToLower())
                                            || l.LastName.ToLower().Contains(keyword.ToLower()));
            }

            return learners;
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