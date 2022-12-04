using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class GetInTouchViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public GetInTouchViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var getInTouch = await _dbContext.GetInTouches.Where(g => !g.IsDeleted).FirstOrDefaultAsync(); 
            return View(getInTouch);
        }
    }
}
