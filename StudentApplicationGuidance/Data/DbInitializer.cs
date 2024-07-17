using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Models;
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

            // Seed courses if the Courses table is empty
            if (!context.Courses.Any())
            {
                SeedCourses(context);
            }

            // Seed subject requirements if the SubjectRequired table is empty
            if (!context.SubjectRequireds.Any())
            {
                SeedSubjectRequired(context);
            }

            // Seed alternative subjects if the AlternativeSubjects table is empty
            if (!context.AlternativeSubjects.Any())
            {
                SeedAlternativeSubjects(context);
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

        private static void SeedCourses(ApplicationDbContext context)
        {
            var courses = new Course[]
            {
                new Course { University = "UKZN", CourseName = "B Sc Computer Science & Information Technology", Points = 34 },
                new Course { University = "DUT", CourseName = "Bachelor of Information and Communications Technology", Points = 30 },
                new Course { University = "MUT", CourseName = "Diploma in Information Technology (ECP)", Points = 23 },
                new Course { University = "MUT", CourseName = "Advanced Diploma in Information Technology", Points = 25 }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();
        }

        private static void SeedSubjectRequired(ApplicationDbContext context)
        {
            var subjectRequired = new SubjectRequired[]
            {
                new SubjectRequired { SubjectId = 1, CourseId = 1, SubjectLevel = 5 },
                new SubjectRequired { SubjectId = 6, CourseId = 1, SubjectLevel = 5 },
                new SubjectRequired { SubjectId = 10, CourseId = 1, SubjectLevel = 5 },
                new SubjectRequired { SubjectId = 1, CourseId = 2, SubjectLevel = 4 },
                new SubjectRequired { SubjectId = 6, CourseId = 2, SubjectLevel = 4 },
                new SubjectRequired { SubjectId = 10, CourseId = 2, SubjectLevel = 4 },
                new SubjectRequired { SubjectId = 1, CourseId = 3, SubjectLevel = 4 },
                new SubjectRequired { SubjectId = 6, CourseId = 3, SubjectLevel = 3 },
                new SubjectRequired { SubjectId = 10, CourseId = 3, SubjectLevel = 3 },
                new SubjectRequired { SubjectId = 1, CourseId = 4, SubjectLevel = 4 },
                new SubjectRequired { SubjectId = 6, CourseId = 4, SubjectLevel = 3 },
                new SubjectRequired { SubjectId = 10, CourseId = 4, SubjectLevel = 3 }
            };

            context.SubjectRequireds.AddRange(subjectRequired);
            context.SaveChanges();
        }

        private static void SeedAlternativeSubjects(ApplicationDbContext context)
        {
            var alternativeSubjects = new AlternativeSubject[]
            {
                new AlternativeSubject { SubjectId = 7, CourseId = 1, AlternativeSubjectName = "Mathematical Literacy", AlternativeSubjectLevel = 5 },
                new AlternativeSubject { SubjectId = 7, CourseId = 2, AlternativeSubjectName = "Mathematical Literacy", AlternativeSubjectLevel = 5 },
                new AlternativeSubject { SubjectId = 7, CourseId = 3, AlternativeSubjectName = "Mathematical Literacy", AlternativeSubjectLevel = 4 },
                new AlternativeSubject { SubjectId = 7, CourseId = 4, AlternativeSubjectName = "Mathematical Literacy", AlternativeSubjectLevel = 4 }
            };

            context.AlternativeSubjects.AddRange(alternativeSubjects);
            context.SaveChanges();
        }
    }
}
