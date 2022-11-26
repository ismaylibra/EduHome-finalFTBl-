using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blog = await _dbContext.Blogs.Where(b => !b.IsDeleted).ToListAsync();
            return View(blog);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
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

            var unicalFileName = await model.Image.GenerateFile(Constants.BlogPath);

            var blog = new Blog
            {
                ImageUrl = unicalFileName,
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                Created = DateTime.Now

            };

            await _dbContext.Blogs.AddAsync(blog);


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var blog = await _dbContext.Blogs.FindAsync(id);
            if (blog is null) return BadRequest();
            var blogUpdateViewModel = new BlogUpdateViewModel
            {


                ImageUrl = blog.ImageUrl,
                Title = blog.Title,
                Author = blog.Author,
                Description = blog.Description
              

            };
            return View(blogUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var blog = await _dbContext.Blogs.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (blog is null) return NotFound();

            if (blog.Id != id) return BadRequest();

            if (model.Image != null)
            {

                if (!ModelState.IsValid)
                {
                    return View(new BlogUpdateViewModel
                    {

                        ImageUrl = blog.ImageUrl,

                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                    return View(new BlogUpdateViewModel
                    {

                        ImageUrl = blog.ImageUrl,

                    });
                }

                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new BlogUpdateViewModel
                    {

                        ImageUrl = blog.ImageUrl,

                    });
                }


                var path = Path.Combine(Constants.BlogPath, "img", "blog", blog.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.BlogPath);
                blog.ImageUrl = unicalFileName;
            }
            blog.Title = model.Title;
            blog.Author = model.Author;
            blog.Description = model.Description;
           


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var blog = await _dbContext.Blogs.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (blog is null) return NotFound();

            if (blog.Id != id) return BadRequest();

            var path = Path.Combine(Constants.TeacherPath, "img", "blog", blog.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
