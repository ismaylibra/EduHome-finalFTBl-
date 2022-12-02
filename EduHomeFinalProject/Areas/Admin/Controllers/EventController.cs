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
                Title = model.Title,
                Content = model.Content,
                Adress = model.Adress,
                StartTime = model.StartTime,
                EndTime = model.EndTime

            };
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (var speakerId in model.SpeakerIds)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
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


        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var eventt = await _dbContext.Events
                .Include(e => e.EventSpeakers)
                .ThenInclude(e => e.Speaker)
                .Where(e => !e.IsDeleted && e.Id == id)
                .FirstOrDefaultAsync();

            if (eventt is null) return NotFound();
            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakerListItem = new List<SelectListItem>();
            speakers.ForEach(s => eventSpeakerListItem.Add(new SelectListItem(s.FullName, s.Id.ToString())));
            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (EventSpeaker eSpeaker in eventt.EventSpeakers)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == eSpeaker.SpeakerId))
                {
                    ModelState.AddModelError("", "Belə spiker mövcud deyil..!");
                    return View();
                }
                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = eSpeaker.Id
                });
            }
            var eventUpdateViewModel = new EventUpdateViewModel
            {
                Title = eventt.Title,
                Content = eventt.Content,
                ImageUrl = eventt.ImageUrl,
                Adress = eventt.Adress,
                Speakers = eventSpeakerListItem,
                SpeakerIds = eventSpeakers.Select(s => s.SpeakerId).ToList(),
                StartTime = eventt.StartTime,
                EndTime = eventt.EndTime
            };

            return View(eventUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var eventt = await _dbContext.Events
                .Include(e => e.EventSpeakers)
                .ThenInclude(e => e.Speaker)
                .Where(e => !e.IsDeleted && e.Id == id)
                .FirstOrDefaultAsync();

            if (eventt is null) return NotFound();
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
            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();
            var speakerList = new List<SelectListItem>();
            if (model.SpeakerIds.Count > 0)
            {
                foreach (int speakerId in model.SpeakerIds)
                {
                    if (!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
                    {
                        ModelState.AddModelError("", "Yanlış spiker seçdiniz..!");
                        return View(model);
                    }
                }
                List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

                foreach (var item in model.SpeakerIds)
                {
                    EventSpeaker eventSpeaker = new EventSpeaker
                    {
                        SpeakerId = item
                    };
                    eventSpeakers.Add(eventSpeaker);
                }
                eventt.EventSpeakers = eventSpeakers;




             }
            else
            {
                ModelState.AddModelError("", "Ən azı 1 spiker seçilməlidir..!");
                return View(model);
            }

            if(model.ImageUrl is not null)
            {


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
                eventt.ImageUrl = unicalFileName;
            }
            eventt.Title = model.Title;
            eventt.Content = model.Content;
            eventt.Adress = model.Adress;  
            eventt.StartTime = model.StartTime;
            eventt.EndTime = model.EndTime;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existedEvent = await _dbContext.Events.FindAsync(id);
            if (existedEvent is null) return NotFound();
            if (existedEvent.Id != id) return NotFound();

            var eventImage = Path.Combine(Constants.RootPath, "assets", "img", "event", existedEvent.ImageUrl);
            if (System.IO.File.Exists(eventImage))
                System.IO.File.Delete(eventImage);

            _dbContext.Events.Remove(existedEvent);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
