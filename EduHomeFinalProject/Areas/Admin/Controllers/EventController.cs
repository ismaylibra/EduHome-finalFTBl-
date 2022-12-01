using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class EventController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public EventController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _dbContext.Events
                .Include(e => e.EventSpeakers)
                .ThenInclude(e => e.Speaker)
                .Where(s => !s.IsDeleted)
                .OrderByDescending(e => e.Id)
                .ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            var speaker = await _dbContext.Speakers.ToListAsync();

            var eventSpeakersListItem = new List<SelectListItem>();

            speaker.ForEach(s => eventSpeakersListItem.Add(new SelectListItem(s.FullName, s.Id.ToString())));
            var model = new EventCreateViewModel
            {
                Speakers = eventSpeakersListItem
            };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartTime) >= 0)
            {
                ModelState.AddModelError("StartTime", "Başlama tarixi gələcəkdə olmalıdı, lakin bitmə vaxtından öncə olmalıdı..!");
                return View(model);
            }

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndTime) >= 0)
            {
                ModelState.AddModelError("EndTime", "Bitiş vaxtı başlama vaxtından sonran olmalıdı..!");
                return View(model);
            }
            if (DateTime.Compare(model.StartTime, model.EndTime) >= 0)
            {
                ModelState.AddModelError("", "Başlama tarixi bitmə tarixindən əvvəl olmalıdı..!");
                return View(model);
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                return View(model);
            }

            if (!model.Image.IsAllowedSize(20))
            {
                ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                return View(model);
            }

            var unicalFileName = await model.Image.GenerateFile(Constants.EventPath);

            var createdEvent = new Event
            {
                ImageUrl = unicalFileName,
                Title =model.Title,
                Content = model.Content,
                Adress = model.Adress,
                StartTime = model.StartTime,
                EndTime = model.EndTime
                
            };
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (var speakerId in model.SpeakerIds)
            {
                if(!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
                {
                    ModelState.AddModelError("", "Belə Spiker yoxdu..!");
                }
                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = speakerId
                });
            }

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            var speakersSelectListItem = new List<SelectListItem>();

            speakers.ForEach(s => speakersSelectListItem.Add(new SelectListItem(s.FullName, s.Id.ToString())));

            createdEvent.EventSpeakers = eventSpeakers;
            model.Speakers = speakersSelectListItem;
            await _dbContext.Events.AddAsync(createdEvent);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


    } }
}
