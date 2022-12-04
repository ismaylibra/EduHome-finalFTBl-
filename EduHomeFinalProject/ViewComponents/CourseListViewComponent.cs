using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class CourseListViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public CourseListViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courses = await _dbContext.Courses.Where(c => !c.IsDeleted).ToListAsync();
            return View(courses);
        }
    }
}
