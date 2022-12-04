using EduHomeFinalProject.Areas.Admin.ViewModels;
using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public ContactController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var contact = await _dbContext.Contacts
                .FirstOrDefaultAsync();

            return View(contact);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();



            await _dbContext.Contacts.AddAsync(new Contact
            {
                Address = model.Address,
                Website = model.Website,
                ContactNumber = model.Number,
                Message = model.Message
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var contact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (contact == null) return BadRequest();

            var model = new ContactUpdateViewModel
            {
                Address = contact.Address,
                Website = contact.Website,
                Number = contact.ContactNumber,
                Message = contact.Message,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ContactUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existContact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (existContact == null) return BadRequest();

           

            if (!ModelState.IsValid) return View(model);

            

            existContact.Message = model.Message;
            existContact.Address = model.Address;
            existContact.Website = model.Website;
            existContact.ContactNumber = model.Number;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existContact = await _dbContext.Contacts
                .Where(contact => contact.Id == id)
                .FirstOrDefaultAsync();

            if (existContact == null) return BadRequest();

            _dbContext.Contacts.Remove(existContact);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
