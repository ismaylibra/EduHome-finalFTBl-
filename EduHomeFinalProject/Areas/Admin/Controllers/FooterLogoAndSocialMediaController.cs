using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class FooterLogoAndSocialMediaController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public FooterLogoAndSocialMediaController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var footerLogo = _dbContext.FooterLogoAndSocialMedias.FirstOrDefault(); 
            return View(footerLogo);
        }
       
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var footerLogo = await _dbContext.FooterLogoAndSocialMedias.Where(f=>!f.IsDeleted && f.Id == id).FirstOrDefaultAsync();
            if(footerLogo is null) return NotFound();

            var footerLogoUpdateViewModel = new FooterLogoAndSocialMediaUpdateViewModel
            {
                ImageUrl = footerLogo.ImageUrl,
                Content = footerLogo.Content,
                FacebookLink = footerLogo.FacebookLink,
                PinterestLink = footerLogo.PinterestLink,
                VimeoLink = footerLogo.VimeoLink,
                Twitterlink = footerLogo.Twitterlink
            };
            return View(footerLogoUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FooterLogoAndSocialMediaUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var footerLogo = await _dbContext.FooterLogoAndSocialMedias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (footerLogo is null) return NotFound();

            if (footerLogo.Id != id) return BadRequest();

            if (model.Image != null)
            {

                if (!ModelState.IsValid)
                {
                    return View(new FooterLogoAndSocialMediaUpdateViewModel
                    {

                        ImageUrl = footerLogo.ImageUrl,

                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                    return View(new FooterLogoAndSocialMediaUpdateViewModel
                    {

                        ImageUrl = footerLogo.ImageUrl,

                    });
                }

                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new FooterLogoAndSocialMediaUpdateViewModel
                    {

                        ImageUrl = footerLogo.ImageUrl,

                    });
                }


                var path = Path.Combine(Constants.RootPath, "img", "logo", footerLogo.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.FooterPath);
                footerLogo.ImageUrl = unicalFileName;
            }
            footerLogo.Content = model.Content;
            footerLogo.FacebookLink = model.FacebookLink;
            footerLogo.PinterestLink = model.PinterestLink;
            footerLogo.Twitterlink = model.Twitterlink;
            footerLogo.VimeoLink = model.VimeoLink;
           


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
