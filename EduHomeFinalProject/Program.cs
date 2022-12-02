using EduHomeFinalProject.DAL;
using EduHomeFinalProject.Data;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.SliderPath = Path.Combine(Constants.RootPath, "assets", "img", "slider");
            Constants.TeacherPath = Path.Combine(Constants.RootPath, "assets", "img", "teacher");
            Constants.BlogPath = Path.Combine(Constants.RootPath, "assets", "img", "blog");
            Constants.CoursePath = Path.Combine(Constants.RootPath, "assets", "img", "course");
            Constants.AboutPath = Path.Combine(Constants.RootPath, "assets", "img", "about");
            Constants.SpeakerPath = Path.Combine(Constants.RootPath, "assets", "img", "speaker");
            Constants.EventPath = Path.Combine(Constants.RootPath, "assets", "img", "event");
            Constants.FooterPath = Path.Combine(Constants.RootPath, "assets", "img", "logo");



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                  name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            
               

            app.Run();
        }
    }
}