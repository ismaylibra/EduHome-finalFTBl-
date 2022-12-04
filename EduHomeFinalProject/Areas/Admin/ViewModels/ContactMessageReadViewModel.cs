using EduHomeFinalProject.DAL.Entities;

namespace EduHomeFinalProject.Areas.Admin.ViewModels
{
    public class ContactMessageReadViewModel
    {
        public List<ContactMessage> ContactMessages { get; set; }
        public bool IsAllReadMessage { get; set; }
    }
}
