using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class CourseController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses.Where(c => !c.IsDeleted).Include(c => c.Category).ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories.Where(c=>!c.IsDeleted).ToListAsync();

            var categoryListItem = new List<SelectListItem>
            {
                new SelectListItem("Category secin" , "0")
            };

            categories.ForEach(c => categoryListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));
            var model = new CourseCreateViewModel
            {
                Categories = categoryListItem
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            var categories = await _dbContext.Categories.Where(c => !c.IsDeleted).Include(c => c.Courses).ToListAsync();
            if (!ModelState.IsValid) return View(model);

            var categoryListItem = new List<SelectListItem>
             {
                 new SelectListItem("Parent Category Secin", "0")
             };

            categories.ForEach(c => categoryListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var viewModel = new CourseCreateViewModel
            {
                Categories= categoryListItem
                 
                
            };

            var createdCourse = new Course();

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

            var unicalFileName = await model.Image.GenerateFile(Constants.CoursePath);

            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("", "Parent kateqori secmelisiz");
                return View();
            }
            createdCourse.ImageUrl = unicalFileName;
            createdCourse.CategoryId = model.CategoryId;
            createdCourse.Title = model.Title;
            createdCourse.SkillLevel = model.SkillLevel;
            createdCourse.Description = model.Description;
            createdCourse.Duration = model.Duration;
            createdCourse.About = model.About;
            createdCourse.ApplyContent = model.ApplyContent;
            createdCourse.CertificationContent = model.CertificationContent;
            createdCourse.StartTime = DateTime.Now;
            createdCourse.SkillLevel = model.SkillLevel;
            createdCourse.Language = model.Language;
            createdCourse.ClassDuration = model.ClassDuration;
            createdCourse.Assesment = model.Assesment;

            await _dbContext.Courses.AddAsync(createdCourse);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();

            var category = await _dbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if (category is null) return NotFound();
            var course = await _dbContext.Courses.Where(c => !c.IsDeleted && c.Id == id).Include(c=>c.Category).FirstOrDefaultAsync();
            if (course is null) return NotFound();

            if (course.Id != id) return BadRequest();

            var selectedCategories = new List<SelectListItem>();

            category.ForEach(c => selectedCategories.Add(new SelectListItem(c.Name, c.Id.ToString())));
            var courseUpdateViewModel = new CourseUpdateViewModel
            {
                Id = course.Id,
                Title = course.Title,
                ImageUrl = course.ImageUrl,
                Description = course.Description,
                About = course.About,
                SkillLevel = course.SkillLevel,
                Language = course.Language,
                Duration = course.Duration,
                ApplyContent = course.ApplyContent,
                CertificationContent = course.CertificationContent,
                ClassDuration = course.ClassDuration,
                CategoryId = course.CategoryId,
                Assesment = course.Assesment,
                Categories = selectedCategories

            };
            return View(courseUpdateViewModel);
           

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Update(int? id, CourseUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (id is null) return NotFound();
            var categories = await _dbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if (categories is null) return NotFound();
            var course = await _dbContext.Courses.Where(c => !c.IsDeleted && c.Id == id).Include(c => c.Category).FirstOrDefaultAsync();
            if (course is null) return NotFound();
           if(model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new CourseUpdateViewModel
                    {

                        ImageUrl = course.ImageUrl,

                    });
                }


                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                    return View(new CourseUpdateViewModel
                    {

                        ImageUrl = course.ImageUrl,

                    });
                }

                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new CourseUpdateViewModel
                    {

                        ImageUrl = course.ImageUrl,
                    });
                }



                var path = Path.Combine(Constants.RootPath, "img", "course", course.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.CoursePath);
                course.ImageUrl = unicalFileName;
            }

            var selectedCategory = new CourseUpdateViewModel
            {
                CategoryId = model.CategoryId,


            };
           




            course.Title = model.Title;
            course.Description = model.Description;
            course.About = model.About;
            course.Assesment = model.Assesment;
            course.ClassDuration = model.ClassDuration;
            course.Duration = model.Duration;
            course.SkillLevel = model.SkillLevel;
            course.Language = model.Language;
            course.StudentCount = model.StudentCount;
            course.ApplyContent = model.ApplyContent;
            course.CertificationContent = model.CertificationContent;
            course.CategoryId = model.CategoryId;             



            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var course = await _dbContext.Courses
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course is null) return NotFound();

            if (course.Id != id) return BadRequest();

            var path = Path.Combine(Constants.RootPath, "assets" ,"img", "course", course.ImageUrl);

            var result = System.IO.File.Exists(path);
            if (result)
            {
                System.IO.File.Delete(path);
            }          
                

            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
