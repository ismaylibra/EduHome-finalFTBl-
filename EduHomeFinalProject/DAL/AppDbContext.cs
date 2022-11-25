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
    }
}
