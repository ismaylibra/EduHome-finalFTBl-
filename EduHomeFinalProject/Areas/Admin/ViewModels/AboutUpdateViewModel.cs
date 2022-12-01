namespace EduHomeFinalProject.Areas.Admin.ViewModels
{
    public class AboutUpdateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public string ButtonContent { get; set; }
        public string ButtonUrl { get; set; }
    }
}
