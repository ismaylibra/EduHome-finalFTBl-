using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public TeacherController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var teacher = await _dbContext.Teachers.Where(t => !t.IsDeleted).ToListAsync();
            return View(teacher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
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

            var teacher = new Teacher
            {
                ImageUrl = unicalFileName,
                FullName = model.FullName,
                Profession=model.Profession,
                About = model.About,
                Degree = model.Degree,
                Experience=model.Experience,
                Hobbies=model.Hobbies,
                Faculty = model.Faculty,
                Mail = model.Mail,
                PhoneNumber=model.PhoneNumber,
                SkypeAdress=model.SkypeAdress,
                Language=model.Language,
                TeamLeader=model.TeamLeader,
                Development=model.Development,
                Design=model.Design,
                Innovation=model.Innovation,
                Communication=model.Communication
                
            };

            await _dbContext.Teachers.AddAsync(teacher);


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var teacher = await _dbContext.Teachers.FindAsync(id);
            if (teacher is null) return BadRequest();
            var teacherUpdateViewModel = new TeacherUpdateViewModel
            {


                ImageUrl = teacher.ImageUrl,
                FullName = teacher.FullName,
                Profession = teacher.Profession,
                About = teacher.About,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty,
                Mail = teacher.Mail,
                PhoneNumber = teacher.PhoneNumber,
                SkypeAdress = teacher.SkypeAdress,
                Language = teacher.Language,
                TeamLeader = teacher.TeamLeader,
                Development = teacher.Development,
                Design = teacher.Design,
                Innovation = teacher.Innovation,
                Communication = teacher.Communication

            };
            return View(teacherUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateViewModel model)
        {
            if (id is null) return NotFound();

            var teacher = await _dbContext.Teachers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (teacher is null) return NotFound();

            if (teacher.Id != id) return BadRequest();

            if (model.Image != null)
            {

                if (!ModelState.IsValid)
                {
                    return View(new TeacherUpdateViewModel
                    {

                        ImageUrl = teacher.ImageUrl,

                    });
                }

                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçilməlidir..!");
                    return View(new TeacherUpdateViewModel
                    {

                        ImageUrl = teacher.ImageUrl,

                    });
                }

                if (!model.Image.IsAllowedSize(50))
                {
                    ModelState.AddModelError("Image", "Şəklin ölçüsü maksimum 20mb ola bilər..!");
                    return View(new TeacherUpdateViewModel
                    {

                        ImageUrl = teacher.ImageUrl,

                    });
                }


                var path = Path.Combine(Constants.TeacherPath, "img", "teacher", teacher.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalFileName = await model.Image.GenerateFile(Constants.TeacherPath);
                teacher.ImageUrl = unicalFileName;
            }
            teacher.FullName = model.FullName;
            teacher.Profession = model.Profession;
            teacher.About = model.About;
            teacher.Degree = model.Degree;
            teacher.Experience = model.Experience;
            teacher.Hobbies = model.Hobbies;
            teacher.Faculty = model.Faculty;
            teacher.Mail = model.Mail;
            teacher.PhoneNumber = model.PhoneNumber;
            teacher.SkypeAdress = model.SkypeAdress;
            teacher.Language = model.Language;
            teacher.TeamLeader = model.TeamLeader;
            teacher.Development = model.Development;
            teacher.Design = model.Design;
            teacher.Innovation = model.Innovation;
            teacher.Communication = model.Communication;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var teacher = await _dbContext.Teachers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (teacher is null) return NotFound();

            if (teacher.Id != id) return BadRequest();

            var path = Path.Combine(Constants.TeacherPath, "img", "teacher", teacher.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Teachers.Remove(teacher);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
