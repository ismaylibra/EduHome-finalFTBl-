using System.ComponentModel.DataAnnotations;

namespace EduHomeFinalProject.DAL.Entities
{
    public class Teacher : Entity
    {
        public string FullName { get; set; }
        public string Profession { get; set; }
        public string About { get; set; }
        public string ImageUrl { get; set; }
        public string Degree { get; set; }
        public byte Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        [EmailAddress]
        public string Mail  { get; set; }
        public string PhoneNumber { get; set; }
        public string SkypeAdress { get; set; }
        public byte Language { get; set; }
        public byte TeamLeader { get; set; }
        public byte Development { get; set; }
        public byte Design { get; set; }
        public byte Innovation { get; set; }
        public byte Communication { get; set; }

    }
}
