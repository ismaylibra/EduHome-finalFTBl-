using EduHomeFinalProject.DAL;
using EduHomeFinalProject.DAL.Entities;
using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

namespace EduHomeFinalProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder =>
                    {
                        builder.MigrationsAssembly(nameof(EduHomeFinalProject));
                    });
            });
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.Configure<AdminUser>(builder.Configuration.GetSection("AdminUser"));
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

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dataInitializer = new DataInitializer(serviceProvider);

                await dataInitializer.SeedData();
            }
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




            await app.RunAsync();
        }
    }
}