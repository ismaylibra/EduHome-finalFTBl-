using EduHomeFinalProject.Areas.Admin.Data;
using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class FooterInformationController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public FooterInformationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var information = _dbContext.FooterInformations.Where(i=>!i.IsDeleted).FirstOrDefault();
            return View(information);
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var information = await _dbContext.FooterInformations.FindAsync(id);
            if (information.Id != id) return BadRequest();

            var existInformation = new FooterInformationUpdateViewModel
            {
                Title = information.Title,
                Admission = information.Admission,
                AcademicCalendar = information.AcademicCalendar,
                 EventList = information.EventList,
                 HostelAndDinning = information.HostelAndDinning,
                 TimeTable = information.TimeTable
            };
            return View(existInformation);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FooterInformationUpdateViewModel model)
        {

            if (id is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var information = await _dbContext.FooterInformations.FindAsync(id);

            if (information is null) return NotFound();
            var isExistTitle = await _dbContext.FooterInformations.AnyAsync(c => c.Title.ToLower() == model.Title.ToLower() && c.Id != id);
            var isExistAdmission = await _dbContext.FooterInformations.AnyAsync(c => c.Admission.ToLower() == model.Admission.ToLower() && c.Id != id);
            var isExistAcademicCalendar = await _dbContext.FooterInformations.AnyAsync(c => c.AcademicCalendar.ToLower() == model.AcademicCalendar.ToLower() && c.Id != id);
            var isExistEventList = await _dbContext.FooterInformations.AnyAsync(c => c.EventList.ToLower() == model.EventList.ToLower() && c.Id != id);
            var isExistHostel = await _dbContext.FooterInformations.AnyAsync(c => c.HostelAndDinning.ToLower() == model.HostelAndDinning.ToLower() && c.Id != id);
            var isExistTimeTable = await _dbContext.FooterInformations.AnyAsync(c => c.TimeTable.ToLower() == model.TimeTable.ToLower() && c.Id != id);



            if (isExistTitle && isExistAdmission && isExistAcademicCalendar && isExistEventList && isExistHostel && isExistTimeTable)
            {
                ModelState.AddModelError("Name", "Daxil etdiyiniz adda başlıq  mövcuddur..!");
                return View(model);
            }
            information.Title = model.Title;
            information.Admission = model.Admission;
            information.AcademicCalendar = model.AcademicCalendar;
            information.EventList = model.EventList;
            information.HostelAndDinning = model.HostelAndDinning; 
            information.TimeTable = model.TimeTable;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


       
    }
}
