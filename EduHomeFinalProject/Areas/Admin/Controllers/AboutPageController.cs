using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class AboutPageController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public AboutPageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var aboutPage = _dbContext.AboutPages.FirstOrDefault();
            return View(aboutPage);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateViewModel model)
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

            var unicalFileName = await model.Image.GenerateFile(Constants.TeacherPath);

            var about = new AboutPage
            {
                ImageUrl = unicalFileName,
                Title= model.Title,
                Description = model.Description,
                ButtonContent = model.ButtonContent,
                ButtonUrl = model.ButtonUrl

            };

            await _dbContext.AboutPages.AddAsync(about);


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var about = await _dbContext.AboutPages.FindAsync(id);
            if (about is null) return BadRequest();
            var aboutUpdateViewModel = new AboutUpdateViewModel
            {


                Title = about.Title,
                Description = about.Description,
                ImageUrl = about.ImageUrl,
                ButtonContent = about.ButtonContent,
                ButtonUrl = about.ButtonUrl

            };
            return View(aboutUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, AboutUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var about = await _dbContext.AboutPages.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (about is null) return NotFound();

            if (about.Id != id) return BadRequest();

            if (model.Image != null)
            {

                if (!ModelState.IsValid)
                {
                    return View(new AboutUpdateViewModel
                    {

                        ImageUrl = about.ImageUrl,

                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                    return View(new AboutUpdateViewModel
                    {

                        ImageUrl = about.ImageUrl,

                    });
                }

                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new AboutUpdateViewModel
                    {

                        ImageUrl = about.ImageUrl,

                    });
                }


                var path = Path.Combine(Constants.RootPath, "img", "about", about.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.AboutPath);
                about.ImageUrl = unicalFileName;
            }
            about.Title = model.Title;
            about.Description = model.Description;
            about.ButtonContent = model.ButtonContent;
            about.ButtonUrl = model.ButtonUrl;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
