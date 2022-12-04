﻿using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Controllers
{
    public class CourseController : Controller
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

        public async Task<IActionResult> Details(int? id)

        {
            if (id is null) return NotFound();
            var course = await _dbContext.Courses.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (course is null) return NotFound();
           
            
            return View(course);
        }

        public async Task<IActionResult> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return NoContent();

            var courses = await _dbContext.Courses
                .Where(course => !course.IsDeleted && course.Title.ToLower().Contains(searchText.ToLower()))
                .ToListAsync();

            var model = new List<Course>();

            courses.ForEach(course => model.Add(new Course
            {
                Id = course.Id,
                Title = course.Title,
                ImageUrl = course.ImageUrl,
            }));

            return PartialView("_CourseSearchPartial", courses);
        }
    }
}
    
