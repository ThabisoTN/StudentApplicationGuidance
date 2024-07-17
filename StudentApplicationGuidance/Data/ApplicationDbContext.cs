using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Models;

namespace StudentApplicationGuidance.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UserSubject> UserSubjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<SubjectRequired> SubjectRequireds { get; set; }
        public DbSet<AlternativeSubject> AlternativeSubjects { get; set; }
    }
}
