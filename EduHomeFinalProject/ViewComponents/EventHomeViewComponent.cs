using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.ViewComponents
{
    public class EventHomeViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public EventHomeViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var eventt = await _dbContext.Events.Where(e => !e.IsDeleted).OrderByDescending(e=>e.Id).ToListAsync();
            return View(eventt);
        }
    }
}
