using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

            if (!context.Provinces.Any())
            {
                SeedProvinces(context);
            }

            // Seed funding sources if the FundingSources table is empty
            if (!context.FundingSources.Any())
            {
                SeedFundingSources(context);
            }
        }


        //university with their courses and required points
        private static void SeedCourses(ApplicationDbContext context)
        {
            var courses = new Course[]
            {
                new Course { University = "University of KwaZulu-Natal", CourseName = "B Sc Computer Science & Information Technology", Points = 34 },
                //new Course { University = "Durban University of Technology", CourseName = "Bachelor of Information and Communications Technology", Points = 30 },
                //new Course { University = "Mangosuthu University of Technology", CourseName = "Diploma in Information Technology (ECP)", Points = 23 },
                //new Course { University = "Mangosuthu University of Technology", CourseName = "Advanced Diploma in Information Technology", Points = 25 }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();
        }
        
        //Course Required subject and level.   
        private static void SeedSubjectRequired(ApplicationDbContext context)
        {
            var subjectRequired = new SubjectRequired[]
            {
                //First course
                new SubjectRequired {SubjectId=7, CourseId=1, SubjectLevel=5},
                new SubjectRequired {SubjectId=9,CourseId=1, SubjectLevel=4},

                //Second course
                //new SubjectRequired {SubjectId=7,CourseId=2, SubjectLevel=4},

                //Third course
                //new SubjectRequired{SubjectId=7, CourseId=3, SubjectLevel=4},
                
            };

            context.SubjectRequireds.AddRange(subjectRequired);
            context.SaveChanges();
        }

        //Course altenative subjects and levels.
        private static void SeedAlternativeSubjects(ApplicationDbContext context)
        {
            var alternativeSubjects = new AlternativeSubject[]
            {
                //First Course
                new AlternativeSubject {SubjectId=19, CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="Gricultural Science"},
                new AlternativeSubject{SubjectId=11,CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="Life Science"}, 
                new AlternativeSubject{ SubjectId=10,CourseId=1, AlternativeSubjectLevel =4, AlternativeSubjectName="Physical Science"},
                new AlternativeSubject{SubjectId=1,CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="English Home Language"}, 
                new AlternativeSubject{SubjectId=2, CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="English First Additional Language"},

             

            };

            context.AlternativeSubjects.AddRange(alternativeSubjects);
            context.SaveChanges();
        }


        //Subjects
        private static void SeedSubjects(ApplicationDbContext context)
        {
            var listofSubjects = new Subject[]
            {
                new Subject { Name = "English Home Language" },
                new Subject {Name = "English First Editional Language"}, 
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
        
        //User source of funding.
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


        //Provinces
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



    }
}
