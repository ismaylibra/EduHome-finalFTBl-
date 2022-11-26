using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blog = await _dbContext.Blogs.Where(b => !b.IsDeleted).ToListAsync();
            return View(blog);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var blog = await _dbContext.Blogs.SingleOrDefaultAsync(b=>b.Id==id);
            if (blog.Id != id) return NotFound();
            
            return View(blog);
        }
    }
}
