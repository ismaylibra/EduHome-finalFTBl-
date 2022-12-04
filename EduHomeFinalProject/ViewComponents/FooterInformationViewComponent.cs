using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class FooterInformationViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public FooterInformationViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var information = await _dbContext.FooterInformations.Where(i => !i.IsDeleted).FirstOrDefaultAsync();
            return View(information);
        }
    }
}
