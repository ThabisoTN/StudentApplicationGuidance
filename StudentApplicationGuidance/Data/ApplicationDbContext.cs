using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZizoAI.Models; // Ensure this using directive is present for UserSubject

namespace StudentApplicationGuidance.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UserSubject> UserSubjects { get; set; } // Ensure DbSet for UserSubject is declared

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships or constraints if needed
            // Example:
            // modelBuilder.Entity<UserSubject>()
            //     .HasKey(us => new { us.SubjectId, us.UserId });
        }
    }
}
