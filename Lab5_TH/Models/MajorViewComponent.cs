using Microsoft.AspNetCore.Mvc;
using Lab5_TH.Data;
using Lab5_TH.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var majors = await GetMajorsAsync();
            return View("RenderMajor", majors);
        }

        private Task<List<Major>> GetMajorsAsync()
        {
            return Task.FromResult(_context.Majors.ToList());
        }
    }
}