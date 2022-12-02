using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public CourseViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> Index()
        {
            var courses = await _dbContext.Courses.Where(c => !c.IsDeleted).ToListAsync();
            return View(courses);
        }
    }
}
