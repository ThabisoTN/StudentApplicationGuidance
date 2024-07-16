using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Models; 

namespace StudentApplicationGuidance.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        public DbSet<Subject> Subjects { get; set; }

        public DbSet<UserSubject> UserSubjects { get; set; }


    }
}
