namespace EduHomeFinalProject.DAL.Entities
{
    public class Speaker:Entity
    {
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Profession { get; set; }
        public string CompanyName { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
    }
}
