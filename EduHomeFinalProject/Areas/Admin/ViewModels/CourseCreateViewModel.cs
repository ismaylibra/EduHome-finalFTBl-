using EduHomeFinalProject.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduHomeFinalProject.Areas.Admin.ViewModels
{
    public class CourseCreateViewModel
    {
        public IFormFile Image { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string ApplyContent { get; set; }
        public string CertificationContent { get; set; }
        public DateTime StartTime { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assesment { get; set; }
    }
}
