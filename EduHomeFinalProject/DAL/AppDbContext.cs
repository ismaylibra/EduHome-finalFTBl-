using EduHomeFinalProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace EduHomeFinalProject.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<AboutPage> AboutPages { get; set; }
        public DbSet<FooterLogoAndSocialMedia> FooterLogoAndSocialMedias { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventSpeaker> EventSpeakers { get; set; }
    }
}
