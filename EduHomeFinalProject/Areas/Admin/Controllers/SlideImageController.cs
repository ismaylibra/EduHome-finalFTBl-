using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class SlideImageController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public SlideImageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliderImage = await _dbContext.SliderImages.Where(p => !p.IsDeleted).ToListAsync();
            return View(sliderImage);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SlideImageCreateViewModel model)
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

            var unicalFileName = await model.Image.GenerateFile(Constants.SliderPath);

            var slider = new SliderImage
            {
                ImageUrl = unicalFileName,
                Title = model.Title,
                Subtitle = model.Subtitle,
                ButtonContent = model.ButtonContent
            };

            await _dbContext.SliderImages.AddAsync(slider);

          
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
          
        public  async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var slideImage = await _dbContext.SliderImages.FindAsync(id);
            if (slideImage is  null) return BadRequest();
            var slideImageUpdateViewModel = new SlideImageUpdateViewModel
            {

               
                ImageUrl = slideImage.ImageUrl,
                Title = slideImage.Title,
                Subtitle = slideImage.Subtitle,
                ButtonContent = slideImage.ButtonContent,
               ButtonLink=slideImage.ButtonLink,

            };
            return View(slideImageUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SlideImageUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var slideImage = await _dbContext.SliderImages.Where(x=>x.Id == id).FirstOrDefaultAsync();

            if (slideImage is null) return NotFound();

            if (slideImage.Id != id) return BadRequest();

            if (model.Image != null)
            {

                if (!ModelState.IsValid)
                {
                    return View(new SlideImageUpdateViewModel
                    {

                        ImageUrl = slideImage.ImageUrl,

                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Sekil secilmelidir");
                    return View(new SlideImageUpdateViewModel
                    {

                        ImageUrl = slideImage.ImageUrl,

                    });
                }

                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Sekilin hecmi max 20mb ola biler");
                    return View(new SlideImageUpdateViewModel
                    {

                        ImageUrl = slideImage.ImageUrl,

                    });
                }


                var path = Path.Combine(Constants.RootPath, "img", "slider", slideImage.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.SliderPath);
                slideImage.ImageUrl = unicalFileName;
            }
            slideImage.Subtitle = model.Subtitle;
            slideImage.Title = model.Title;
            slideImage.ButtonContent = model.ButtonContent;
            slideImage.ButtonLink = model.ButtonLink;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var slideImage = await _dbContext.SliderImages.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (slideImage is null) return NotFound();

            if (slideImage.Id != id) return BadRequest();

            var path = Path.Combine(Constants.RootPath, "img", "slider", slideImage.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

             _dbContext.SliderImages.Remove(slideImage);
           await  _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
