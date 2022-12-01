using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class FooterLogoAndSocialMediaController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public FooterLogoAndSocialMediaController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var footerLogo = _dbContext.FooterLogoAndSocialMedias.FirstOrDefault(); 
            return View(footerLogo);
        }
    }
}
