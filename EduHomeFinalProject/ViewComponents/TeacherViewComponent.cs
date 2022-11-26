using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public TeacherViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teacher = await _dbContext.Teachers.Where(t => !t.IsDeleted).ToListAsync();
            return View(teacher);
        }
    }
}
