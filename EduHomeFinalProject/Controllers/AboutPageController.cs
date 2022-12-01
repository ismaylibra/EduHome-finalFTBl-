using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EduHomeFinalProject.Controllers
{
    public class AboutPageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AboutPageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var aboutPage = _dbContext.AboutPages.FirstOrDefault();
            return View(aboutPage);
        }
    }
}
