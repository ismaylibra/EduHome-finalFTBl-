using EduHomeFinalProject.DAL;
using EduHomeFinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class BlogSidebarViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public BlogSidebarViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _dbContext.Categories.Where(c => !c.IsDeleted).Include(c => c.Courses).ToListAsync();
            var blogs = await _dbContext.Blogs.Where(b => !b.IsDeleted).OrderByDescending(b => b.Id).ToListAsync();
            var model = new BlogSidebarViewModel
            {
                Categories = categories,
                Blogs = blogs
            };
            return View(model);
            
        }
    }
}
