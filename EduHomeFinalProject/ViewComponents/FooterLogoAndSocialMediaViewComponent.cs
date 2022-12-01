using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class FooterLogoAndSocialMediaViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public FooterLogoAndSocialMediaViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerLogo = await _dbContext.FooterLogoAndSocialMedias.FirstOrDefaultAsync();
            return View(footerLogo);
        }
    }
}
