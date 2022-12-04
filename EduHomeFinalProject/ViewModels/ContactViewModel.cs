using EduHomeFinalProject.DAL.Entities;

namespace EduHomeFinalProject.ViewModels
{
    public class ContactViewModel
    {
        public Contact Contact { get; set; } = new();
        public ContactMessageViewModel ContactMessage { get; set; }
    }
}
