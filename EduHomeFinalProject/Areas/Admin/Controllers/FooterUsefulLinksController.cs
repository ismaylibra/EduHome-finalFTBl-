using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class FooterUsefulLinksController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public FooterUsefulLinksController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var usefulLinks = _dbContext.FooterUsefulLinks.Where(u => !u.IsDeleted).FirstOrDefault();
            return View(usefulLinks);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var usefulLink = await _dbContext.FooterUsefulLinks.FindAsync(id);
            if (usefulLink.Id != id) return BadRequest();

            var existUsefulLink = new FooterUsefulLinksUpdateViewModel
            {
                Title = usefulLink.Title,
                OurCourses = usefulLink.OurCourses,
                AboutUs = usefulLink.AboutUs,
                TeachersAndFaculty = usefulLink.TeachersAndFaculty,
                TeamsAndConditions = usefulLink.TeamsAndConditions,
                OurEvents = usefulLink.OurEvents
            };
            return View(existUsefulLink);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FooterUsefulLinksUpdateViewModel model)
        {

            if (id is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var usefulLink = await _dbContext.FooterUsefulLinks.FindAsync(id);

            if (usefulLink is null) return NotFound();
            var isExistTitle = await _dbContext.FooterUsefulLinks.AnyAsync(c => c.Title.ToLower() == model.Title.ToLower() && c.Id != id);
            var isExistOurCourse = await _dbContext.FooterUsefulLinks.AnyAsync(c => c.OurCourses.ToLower() == model.OurCourses.ToLower() && c.Id != id);
            var isExistAboutUs = await _dbContext.FooterUsefulLinks.AnyAsync(c => c.AboutUs.ToLower() == model.AboutUs.ToLower() && c.Id != id);
            var isExistTeacherAndFaculty = await _dbContext.FooterUsefulLinks.AnyAsync(c => c.TeachersAndFaculty.ToLower() == model.TeachersAndFaculty.ToLower() && c.Id != id);
            var isExistTeamsAndCondition = await _dbContext.FooterUsefulLinks.AnyAsync(c => c.TeamsAndConditions.ToLower() == model.TeamsAndConditions.ToLower() && c.Id != id);
            var isExistOurEvent = await _dbContext.FooterUsefulLinks.AnyAsync(c => c.OurEvents.ToLower() == model.OurEvents.ToLower() && c.Id != id);



            if (isExistTitle && isExistOurCourse && isExistAboutUs && isExistTeacherAndFaculty && isExistTeamsAndCondition && isExistOurEvent)
            {
                ModelState.AddModelError("Name", "Daxil etdiyiniz adda başlıq  mövcuddur..!");
                return View(model);
            }
            usefulLink.Title = model.Title;
            usefulLink.OurCourses = model.OurCourses;
            usefulLink.AboutUs = model.AboutUs;
            usefulLink.TeachersAndFaculty = model.TeachersAndFaculty;
            usefulLink.TeamsAndConditions = model.TeamsAndConditions;
            usefulLink.OurEvents = model.OurEvents;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
