namespace EduHomeFinalProject.Areas.Admin.ViewModels
{
    public class SlideImageCreateViewModel
    {
        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ButtonContent { get; set; }
        public string ButtonLink { get; set; }

    }
}
