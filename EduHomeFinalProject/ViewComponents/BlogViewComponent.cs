using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public BlogViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blog = await _dbContext.Blogs.Where(b => !b.IsDeleted).ToListAsync();
            return View(blog);
        }
    }
}
