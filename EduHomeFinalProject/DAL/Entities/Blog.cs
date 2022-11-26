namespace EduHomeFinalProject.DAL.Entities
{
    public class Blog:Entity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; } 
        public string Description { get; set; }
    }
}
