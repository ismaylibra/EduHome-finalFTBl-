using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class GetInTouchController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public GetInTouchController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var getInTouch = _dbContext.GetInTouches.Where(g => !g.IsDeleted).FirstOrDefault();

            return View(getInTouch);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var getInTouch = await _dbContext.GetInTouches.FindAsync(id);
            if (getInTouch.Id != id) return BadRequest();

            var existGetInTouch = new GetInTouchUpdateViewModel
            {
                Title = getInTouch.Title,
                Adress = getInTouch.Adress,
                FirstPhoneNumber = getInTouch.FirstPhoneNumber,
                SecondPhoneNumber = getInTouch.SecondPhoneNumber,
                Email = getInTouch.Email,
                Website = getInTouch.Website
            };
            return View(existGetInTouch);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, GetInTouchUpdateViewModel model)
        {

            if (id is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var getInTouch = await _dbContext.GetInTouches.FindAsync(id);

            if (getInTouch is null) return NotFound();
            getInTouch.Title = model.Title;
            getInTouch.Adress = model.Adress;
            getInTouch.FirstPhoneNumber = model.FirstPhoneNumber;
            getInTouch.SecondPhoneNumber = model.SecondPhoneNumber;
            getInTouch.Email = model.Email;
            getInTouch.Website = model.Website;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
