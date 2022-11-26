using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _dbContext;

        public TeacherController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult>  Index()
        {
            var teacher = await _dbContext.Teachers.Where(t => !t.IsDeleted).ToListAsync();
            return View(teacher);
        }
        public async Task<IActionResult> Details(int? id)
        {   if (id is null) return NotFound();
            var teacher = await _dbContext.Teachers.SingleOrDefaultAsync(x=> x.Id == id);
            if (teacher.Id != id) return NotFound();
            return View(teacher);
        }
    }
}
