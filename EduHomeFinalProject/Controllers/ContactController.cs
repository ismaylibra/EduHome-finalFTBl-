﻿using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduHomeFinalProject.ViewModels;

namespace EduHomeFinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ContactController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var contact = await _dbContext.Contacts.
                Where(contact => !contact.IsDeleted)
                .FirstOrDefaultAsync();

            var model = new ContactViewModel
            {
                Contact = contact,
                ContactMessage = new ContactMessageViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactMessageViewModel contactMessage)
        {


            if (!ModelState.IsValid)
                return View(nameof(Index), contactMessage);

            var message = new ContactMessage
            {
                Email = contactMessage.Email,
                Name = contactMessage.Name,
                Subject = contactMessage.Subject,
                Message = contactMessage.Message,
                IsRead = false
            };

            await _dbContext.ContactMessages.AddAsync(message);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
