using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class UsefulLinksViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public UsefulLinksViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usefulLink = await _dbContext.FooterUsefulLinks.Where(u => !u.IsDeleted).FirstOrDefaultAsync();
            return View(usefulLink);
        }
    }
}
