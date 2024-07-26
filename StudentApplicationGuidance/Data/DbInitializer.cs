using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Models;



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
                new Course { University = "Durban University of Technology", CourseName = "Bachelor of Information and Communications Technology", Points = 30 },
                new Course { University = "Durban University of Technology", CourseName = "Bachelor of Information and Communications Technology in Internet of Things (IoT)", Points = 28 },
                new Course { University = "Durban University of Technology", CourseName = "Dip ICT: Applications Development", Points = 26 },
                new Course { University = "Durban University of Technology", CourseName = "Dip ICT: Applications Development (4 year Foundation programme)", Points = 26 },
                new Course { University = "Durban University of Technology", CourseName = "Dip ICT: Business Analysis", Points = 26 },
                new Course { University = "Mangosuthu University of Technology", CourseName = "Diploma in Information Technology (ECP)", Points = 23 },
                new Course{University="Mangosuthu University of Technology", CourseName="Diploma in Information Technology", Points=24},
                new Course { University = "Mangosuthu University of Technology", CourseName = "Advanced Diploma in Information Technology", Points = 25 },
               

            };

            context.Courses.AddRange(courses);
            context.SaveChanges();
        }

        //Course Required subject and level.   
        private static void SeedSubjectRequired(ApplicationDbContext context)
        {
            var subjectRequired = new List<SubjectRequired>
    {
        // Required subjects for B Sc Computer Science & Information Technology at UKZN
        new SubjectRequired { SubjectId = 3, CourseId = 1, SubjectLevel = 5 }, // Mathematics
        new SubjectRequired{SubjectId=9, CourseId=1, SubjectLevel=4 },//life orientation
       

        // Required subjects for Bachelor of Information and Communications Technology at DUT
        new SubjectRequired { SubjectId = 3, CourseId = 2, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired{SubjectId=9, CourseId=2, SubjectLevel=4 },//life orientation
       

         new SubjectRequired { SubjectId = 3, CourseId = 3, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired{SubjectId=9, CourseId=3, SubjectLevel=4 },//life orientation
      
        // Required subjects for Dip ICT: Applications Development at DUT
        new SubjectRequired { SubjectId = 3, CourseId = 3, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired { SubjectId = 1, CourseId = 3, SubjectLevel = 4 }, // English Home Language

        // Required subjects for Dip ICT: Applications Development (4 year Foundation programme) at DUT
        new SubjectRequired { SubjectId = 3, CourseId = 4, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired { SubjectId = 1, CourseId = 4, SubjectLevel = 4 }, // English Home Language

        // Required subjects for Dip ICT: Business Analysis at DUT
        new SubjectRequired { SubjectId = 3, CourseId = 5, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired { SubjectId = 1, CourseId = 5, SubjectLevel = 4 }, // English Home Language

        // Required subjects for Diploma in Information Technology at MUT
        new SubjectRequired { SubjectId = 3, CourseId = 7, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired { SubjectId = 1, CourseId = 7, SubjectLevel = 4 }, // English Home Language

        // Required subjects for Diploma in Information Technology (ECP) at MUT
        new SubjectRequired { SubjectId = 3, CourseId = 8, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired { SubjectId = 1, CourseId = 8, SubjectLevel = 4 }, // English Home Language

        // Required subjects for Advanced Diploma in Information Technology at MUT
        new SubjectRequired { SubjectId = 3, CourseId = 9, SubjectLevel = 4 }, // Mathematics
        new SubjectRequired { SubjectId = 1, CourseId = 9, SubjectLevel = 4 }, // English Home Language
        new SubjectRequired { SubjectId = 10, CourseId = 9, SubjectLevel = 4 }, // Physical Science
    };

            context.SubjectRequireds.AddRange(subjectRequired);
            context.SaveChanges();
        }
        //Course altenative subjects and levels.
        private static void SeedAlternativeSubjects(ApplicationDbContext context)
        {
            var alternativeSubjects = new List<AlternativeSubject>
    {
        // Alternative subjects for B Sc Computer Science & Information Technology at UKZN
        new AlternativeSubject { SubjectId = 1, CourseId = 1, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 1, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 1, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 1, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 1, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },

        // Alternative subjects for Bachelor of Information and Communications Technology at DUT
        new AlternativeSubject { SubjectId = 1, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
         new AlternativeSubject { SubjectId = 2, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
         new AlternativeSubject { SubjectId = 19, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
         new AlternativeSubject { SubjectId = 11, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
         new AlternativeSubject { SubjectId = 10, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
         new AlternativeSubject { SubjectId = 3, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Geography" },
         new AlternativeSubject { SubjectId = 4, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "History" },
         new AlternativeSubject { SubjectId = 5, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Accounting" },
         new AlternativeSubject { SubjectId = 6, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Business Studies" },
         new AlternativeSubject { SubjectId = 7, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Economics" },
         new AlternativeSubject { SubjectId = 8, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Information Technology" },
         new AlternativeSubject { SubjectId = 9, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Computer Applications Technology" },
         new AlternativeSubject { SubjectId = 12, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Technology" },
         new AlternativeSubject { SubjectId = 13, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Management Practices" },
         new AlternativeSubject { SubjectId = 14, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Tourism" },
         new AlternativeSubject { SubjectId = 15, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Hospitality Studies" },
         new AlternativeSubject { SubjectId = 16, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Consumer Studies" },
         new AlternativeSubject { SubjectId = 17, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Engineering Graphics and Design" },
         new AlternativeSubject { SubjectId = 18, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Visual Arts" },
         new AlternativeSubject { SubjectId = 20, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dance Studies" },
         new AlternativeSubject { SubjectId = 21, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Design" },
         new AlternativeSubject { SubjectId = 22, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dramatic Arts" },
         new AlternativeSubject { SubjectId = 23, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Music" },

        //Bachelor of Information and Communications Technology in Internet of Things (IoT)
         new AlternativeSubject { SubjectId = 1, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
         new AlternativeSubject { SubjectId = 2, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
         new AlternativeSubject { SubjectId = 19, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
         new AlternativeSubject { SubjectId = 11, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
         new AlternativeSubject { SubjectId = 10, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
         new AlternativeSubject { SubjectId = 3, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Geography" },
         new AlternativeSubject { SubjectId = 4, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "History" },
         new AlternativeSubject { SubjectId = 5, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Accounting" },
         new AlternativeSubject { SubjectId = 6, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Business Studies" },
         new AlternativeSubject { SubjectId = 7, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Economics" },
         new AlternativeSubject { SubjectId = 8, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Information Technology" },
         new AlternativeSubject { SubjectId = 9, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Computer Applications Technology" },
         new AlternativeSubject { SubjectId = 12, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Technology" },
         new AlternativeSubject { SubjectId = 13, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Management Practices" },
         new AlternativeSubject { SubjectId = 14, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Tourism" },
         new AlternativeSubject { SubjectId = 15, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Hospitality Studies" },
         new AlternativeSubject { SubjectId = 16, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Consumer Studies" },
         new AlternativeSubject { SubjectId = 17, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Engineering Graphics and Design" },
         new AlternativeSubject { SubjectId = 18, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Visual Arts" },
         new AlternativeSubject { SubjectId = 20, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dance Studies" },
         new AlternativeSubject { SubjectId = 21, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Design" },
         new AlternativeSubject { SubjectId = 22, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dramatic Arts" },
         new AlternativeSubject { SubjectId = 23, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Music" },


        // Alternative subjects for Dip ICT: Applications Development at DUT
        new AlternativeSubject { SubjectId = 1, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 3, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },

        // Alternative subjects for Dip ICT: Applications Development (4 year Foundation programme) at DUT
        new AlternativeSubject { SubjectId = 1, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },

        // Alternative subjects for Dip ICT: Business Analysis at DUT
        new AlternativeSubject { SubjectId = 1, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },

        // Alternative subjects for Diploma in Information Technology (ECP) at MUT
        new AlternativeSubject { SubjectId = 1, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },

        // Alternative subjects for Diploma in Information Technology at MUT
        new AlternativeSubject { SubjectId = 1, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },

        // Alternative subjects for Advanced Diploma in Information Technology at MUT
        new AlternativeSubject { SubjectId = 1, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },

        // Alternative subjects for Bachelor of Information and Communications Technology in Internet of Things (IoT) at DUT
        new AlternativeSubject { SubjectId = 1, CourseId = 9, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        new AlternativeSubject { SubjectId = 2, CourseId = 9, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        new AlternativeSubject { SubjectId = 10, CourseId = 9, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        new AlternativeSubject { SubjectId = 11, CourseId = 9, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        new AlternativeSubject { SubjectId = 19, CourseId = 9, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
    };

            // Check if the alternative subjects already exist before adding them to avoid duplicates
            foreach (var altSubject in alternativeSubjects)
            {
                if (!context.AlternativeSubjects.Any(a => a.SubjectId == altSubject.SubjectId && a.CourseId == altSubject.CourseId))
                {
                    context.AlternativeSubjects.Add(altSubject);
                }
            }

            context.SaveChanges();
        }

        // //First Course
        // new AlternativeSubject{SubjectId=1,CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="English Home Language"},
        // new AlternativeSubject{SubjectId=2, CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="English First Additional Language"},
        // new AlternativeSubject {SubjectId=19, CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="Gricultural Science"},
        // new AlternativeSubject{SubjectId=11,CourseId=1, AlternativeSubjectLevel=4, AlternativeSubjectName="Life Science"}, 
        // new AlternativeSubject{ SubjectId=10,CourseId=1, AlternativeSubjectLevel =4, AlternativeSubjectName="Physical Science"},


        // //second course
        //new AlternativeSubject { SubjectId = 1, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
        // new AlternativeSubject { SubjectId = 2, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
        // new AlternativeSubject { SubjectId = 19, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
        // new AlternativeSubject { SubjectId = 11, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
        // new AlternativeSubject { SubjectId = 10, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
        // new AlternativeSubject { SubjectId = 3, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Geography" },
        // new AlternativeSubject { SubjectId = 4, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "History" },
        // new AlternativeSubject { SubjectId = 5, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Accounting" },
        // new AlternativeSubject { SubjectId = 6, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Business Studies" },
        // new AlternativeSubject { SubjectId = 7, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Economics" },
        // new AlternativeSubject { SubjectId = 8, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Information Technology" },
        // new AlternativeSubject { SubjectId = 9, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Computer Applications Technology" },
        // new AlternativeSubject { SubjectId = 12, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Technology" },
        // new AlternativeSubject { SubjectId = 13, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Management Practices" },
        // new AlternativeSubject { SubjectId = 14, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Tourism" },
        // new AlternativeSubject { SubjectId = 15, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Hospitality Studies" },
        // new AlternativeSubject { SubjectId = 16, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Consumer Studies" },
        // new AlternativeSubject { SubjectId = 17, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Engineering Graphics and Design" },
        // new AlternativeSubject { SubjectId = 18, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Visual Arts" },
        // new AlternativeSubject { SubjectId = 20, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dance Studies" },
        // new AlternativeSubject { SubjectId = 21, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Design" },
        // new AlternativeSubject { SubjectId = 22, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dramatic Arts" },
        // new AlternativeSubject { SubjectId = 23, CourseId = 2, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Music" }


        //Subjects
        private static void SeedSubjects(ApplicationDbContext context)
        {
            var listofSubjects = new Subject[]
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

            // Check if the subjects already exist before adding them to avoid duplicates
            foreach (var subject in listofSubjects)
            {
                if (!context.Subjects.Any(s => s.Name == subject.Name))
                {
                    context.Subjects.Add(subject);
                }
            }

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
