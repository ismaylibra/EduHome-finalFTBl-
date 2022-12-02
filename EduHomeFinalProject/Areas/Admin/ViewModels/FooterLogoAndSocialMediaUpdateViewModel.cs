namespace EduHomeFinalProject.Areas.Admin.ViewModels
{
    public class FooterLogoAndSocialMediaUpdateViewModel
    {
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string Content { get; set; }
        public string FacebookLink { get; set; }
        public string PinterestLink { get; set; }
        public string VimeoLink { get; set; }
        public string Twitterlink { get; set; }
    }
}
