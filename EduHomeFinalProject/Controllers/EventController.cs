using EduHomeFinalProject.DAL;
using EduHomeFinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _dbContext;

        public EventController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _dbContext.Events.Where(e => !e.IsDeleted).ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var eventt = await _dbContext.Events.Where(c => !c.IsDeleted && c.Id == id).Include(e=>e.EventSpeakers).ThenInclude(e=>e.Speaker).FirstOrDefaultAsync();
            if (eventt is null) return NotFound();

            
            return View(eventt);
        }
    }
}
