using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StudentApplicationGuidance.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure all pending migrations are applied
            context.Database.Migrate();

            // Seed subjects if the Subjects table is empty
            if (!context.Subjects.Any())
            {
                SeedSubjects(context);
            }
        }

        private static void SeedSubjects(ApplicationDbContext context)
        {
            var listofSubjects = new Subject[]
            {
                new Subject { Name = "English Home Language" },
                new Subject { Name = "Afrikaans Home Language" },
                new Subject { Name = "Afrikaans First Additional Language" },
                new Subject { Name = "IsiZulu Home Language" },
                new Subject { Name = "IsiZulu First Additional Language" },
                new Subject { Name = "Mathematics" },
                new Subject { Name = "Mathematical Literacy" },
                new Subject { Name = "Life Orientation" },
                new Subject { Name = "Physical Science" },
                new Subject { Name = "Life Sciences" },
                new Subject { Name = "Geography" },
                new Subject { Name = "History" },
                new Subject { Name = "Accounting" },
                new Subject { Name = "Business Studies" },
                new Subject { Name = "Economics" },
                new Subject { Name = "Information Technology" },
                new Subject { Name = "Computer Applications Technology" },
                new Subject { Name = "Agricultural Science" },
                new Subject { Name = "Agricultural Technology" },
                new Subject { Name = "Agricultural Management Practices" },
                new Subject { Name = "Tourism" },
                new Subject { Name = "Hospitality Studies" },
                new Subject { Name = "Consumer Studies" },
                new Subject { Name = "Engineering Graphics and Design" },
                new Subject { Name = "Visual Arts" },
                new Subject { Name = "Dance Studies" },
                new Subject { Name = "Design" },
                new Subject { Name = "Dramatic Arts" },
                new Subject { Name = "Music" },
            };

            context.Subjects.AddRange(listofSubjects);
            context.SaveChanges();
        }
    }
}
