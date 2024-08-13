using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using StudentApplicationGuidance.Services;
using System;

namespace StudentApplicationGuidance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration
            builder.Configuration.AddJsonFile("appsettings.json");

            // Database configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity configuration
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add SubjectServices to DI container
            builder.Services.AddScoped<SubjectService>();
            builder.Services.AddScoped<UserSubjectService>();
            builder.Services.AddScoped<CourseQualificationService>();
            builder.Services.AddScoped<TutorAIService>();

            // Controllers and Views configuration
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Database seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // HTTP request pipeline configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint(); // Enable database management endpoint in development
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Error handling middleware in production
                app.UseHsts(); // HTTP Strict Transport Security (HSTS) for enhanced security
            }

            // Middleware for HTTPS redirection, static files, routing, and authorization
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Default routing configuration
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Razor Pages configuration
            app.MapRazorPages();

            app.Run();
        }
    }
}
