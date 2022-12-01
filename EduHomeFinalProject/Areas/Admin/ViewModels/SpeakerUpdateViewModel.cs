namespace EduHomeFinalProject.Areas.Admin.ViewModels
{
    public class SpeakerUpdateViewModel
    {
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Profession { get; set; }
        public string CompanyName { get; set; }
    }
}
