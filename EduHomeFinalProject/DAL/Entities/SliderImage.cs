using System.ComponentModel.DataAnnotations.Schema;

namespace EduHomeFinalProject.DAL.Entities
{
    public class SliderImage : Entity
    {

        public string? ImageUrl { get; set; }   
        public string ButtonLink { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ButtonContent { get; set; }
    }
}
