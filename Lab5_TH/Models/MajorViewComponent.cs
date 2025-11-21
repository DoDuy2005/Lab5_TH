using Lab5_TH.Data;

using Microsoft.AspNetCore.Mvc;
using Lab5_TH.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab5_TH.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        private readonly SchoolContext _context;

        public MajorViewComponent(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var majors = await _context.Majors.ToListAsync();
            // Thêm item "All Majors"
            var allMajors = new List<object>
            {
                new { MajorID = 0, MajorName = "All Majors" }
            };
            allMajors.AddRange(majors.Select(m => new { m.MajorID, m.MajorName }));

            return View(allMajors);
        }
    }
}