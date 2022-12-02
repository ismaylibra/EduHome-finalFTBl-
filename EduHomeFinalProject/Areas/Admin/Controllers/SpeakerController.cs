using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class SpeakerController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public SpeakerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var speaker = await _dbContext.Speakers.Where(t => !t.IsDeleted).ToListAsync();
            return View(speaker);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

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

            var unicalFileName = await model.Image.GenerateFile(Constants.SpeakerPath);

            var speaker = new Speaker
            {
                ImageUrl = unicalFileName,
                FullName = model.FullName,
                Profession = model.Profession,
                CompanyName = model.CompanyName

            };

            await _dbContext.Speakers.AddAsync(speaker);


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var speaker = await _dbContext.Speakers.FindAsync(id);
            if (speaker is null) return BadRequest();
            var speakerUpdateViewModel = new SpeakerUpdateViewModel
            {


                ImageUrl = speaker.ImageUrl,
                FullName = speaker.FullName,
                Profession = speaker.Profession,
                CompanyName = speaker.CompanyName

            };
            return View(speakerUpdateViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var speaker = await _dbContext.Speakers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (speaker is null) return NotFound();

            if (speaker.Id != id) return BadRequest();

            if (model.Image != null)
            {

                if (!ModelState.IsValid)
                {
                    return View(new SpeakerUpdateViewModel
                    {

                        ImageUrl = speaker.ImageUrl,

                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                    return View(new SpeakerUpdateViewModel
                    {

                        ImageUrl = speaker.ImageUrl,

                    });
                }

                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new SpeakerUpdateViewModel
                    {

                        ImageUrl = speaker.ImageUrl,

                    });
                }


                var path = Path.Combine(Constants.RootPath, "img", "speaker", speaker.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.SpeakerPath);
                speaker.ImageUrl = unicalFileName;
            }
            speaker.FullName = model.FullName;
            speaker.Profession = model.Profession;
            speaker.CompanyName = model.CompanyName;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var speaker = await _dbContext.Speakers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (speaker is null) return NotFound();

            if (speaker.Id != id) return BadRequest();

            var path = Path.Combine(Constants.RootPath, "img", "speaker", speaker.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Speakers.Remove(speaker);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
