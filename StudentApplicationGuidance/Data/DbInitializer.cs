using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Models;
using System;

namespace StudentApplicationGuidance.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (!context.SAUniversities.Any())
            {
                SeedSAUniversities(context);
            }

            if (!context.Subjects.Any())
            {
                SeedSubjects(context);
            }

            if (!context.Provinces.Any())
            {
                SeedProvinces(context);
            }

            if (!context.FundingSources.Any())
            {
                SeedFundingSources(context);
            }
        }

        private static void SeedSAUniversities(ApplicationDbContext context)
        {
            try
            {
                var sauniversities = new SAUniversities[]
                {
                    new SAUniversities { UniversityName = "University of Cape Town" },
                    new SAUniversities { UniversityName = "Durban University of Technology" },
                    new SAUniversities { UniversityName = "Mangosuthu University of Technology" }
                };

                context.SAUniversities.AddRange(sauniversities);
                context.SaveChanges();
                Console.WriteLine("Universities seeded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding universities: {ex.Message}");
            }
        }

        private static void SeedSubjects(ApplicationDbContext context)
        {
            var subjects = new Subject[]
            {
                new Subject { Name = "English Home Language" },
                new Subject { Name = "English First Additional Language" },
                new Subject { Name = "Mathematics" },
                new Subject { Name = "Mathematical Literacy" },
                new Subject { Name = "IsiZulu Home Language" },
                new Subject { Name = "IsiZulu First Additional Language" },
                new Subject { Name = "Afrikaans Home Language" },
                new Subject { Name = "Afrikaans First Additional Language" },
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

            context.Subjects.AddRange(subjects);
            context.SaveChanges();
        }


        private static void SeedProvinces(ApplicationDbContext context)
        {
            var provinces = new Province[]
            {
                new Province { Name = "Eastern Cape" },
                new Province { Name = "Free State" },
                new Province { Name = "Gauteng" },
                new Province { Name = "KwaZulu-Natal" },
                new Province { Name = "Limpopo" },
                new Province { Name = "Mpumalanga" },
                new Province { Name = "Northern Cape" },
                new Province { Name = "North West" },
                new Province { Name = "Western Cape" }
            };

            context.Provinces.AddRange(provinces);
            context.SaveChanges();
        }

        private static void SeedFundingSources(ApplicationDbContext context)
        {
            var fundingSources = new FundingSource[]
            {
                new FundingSource { Name = "NSFAS" },
                new FundingSource { Name = "Bursary" },
                new FundingSource { Name = "Self-funded" },
                new FundingSource { Name = "Scholarship" }
            };

            context.FundingSources.AddRange(fundingSources);
            context.SaveChanges();
        }
    }
}
