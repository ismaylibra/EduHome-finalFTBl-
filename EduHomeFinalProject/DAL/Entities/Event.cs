using System.ComponentModel.DataAnnotations;

namespace EduHomeFinalProject.DAL.Entities
{
    public class Event:Entity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }
        public string Adress { get; set; }
        public string Content { get; set; }

        public List<EventSpeaker> EventSpeakers { get; set; }
    }
}
