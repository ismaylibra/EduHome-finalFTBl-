using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class CategoryController : BaseController 
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _dbContext.Categories.Where(c=>!c.IsDeleted).ToListAsync();
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existCategory = await _dbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();

            if (existCategory.Any(c => c.Name.ToLower().Equals(model.Name.ToLower()))) {
                 ModelState.AddModelError("Name", "Bu adda kateqoriya mövcuddur..! ");
                return View();
            }
            var newCategory = new Category
            {
                Name = model.Name
            };

            await _dbContext.Categories.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var category = await _dbContext.Categories.FindAsync(id);
            if (category.Id != id) return BadRequest();

            var existCategory = new CategoryUpdateViewModel
            {
                Name = category.Name
            };
            return View(existCategory);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,  CategoryUpdateViewModel model)
        {

            if (id is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null) return NotFound();
            var isExistName = await _dbContext.Categories.AnyAsync(c => c.Name.ToLower() == model.Name.ToLower() && c.Id != id);

            if (isExistName)
            {
                ModelState.AddModelError("Name", "Daxil etdiyiniz adda kateqoriya  mövcuddur..!");
                return View(model);
            }
            category.Name = model.Name;
           
            await _dbContext.SaveChangesAsync();
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null) return NotFound();

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
