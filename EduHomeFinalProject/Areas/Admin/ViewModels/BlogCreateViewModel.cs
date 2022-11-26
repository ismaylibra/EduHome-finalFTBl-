namespace EduHomeFinalProject.Areas.Admin.ViewModels
{
    public class BlogCreateViewModel
    {   public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
    }
}
