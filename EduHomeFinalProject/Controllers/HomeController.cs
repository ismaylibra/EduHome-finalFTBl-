using EduHomeFinalProject.DAL;
using EduHomeFinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHomeFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public async Task<IActionResult> Index()
        {
            var slider = await _dbContext.SliderImages.ToListAsync();

            var homeViewModel = new HomeViewModel
            {
                    SliderImages = slider
            };
            return View(homeViewModel);
        }

      
        
    }
}