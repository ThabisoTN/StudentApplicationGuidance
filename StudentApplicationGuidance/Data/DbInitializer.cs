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

            //Seed university table
            if(!context.universities.Any())
            {
                SeedUniversity(context);
            }
        }

        public static void SeedUniversity(ApplicationDbContext context)
        {
            // Check if any universities exist in the database
            if (!context.universities.Any())
            {
                // Define the list of universities to seed
                var universities = new University[]
                {
            new University { UniversityName = "University of Cape Town" },
            new University { UniversityName = "University of the Witwatersrand" },
            new University { UniversityName = "University of Pretoria" },
            new University { UniversityName = "University of Stellenbosch" },
            new University { UniversityName = "University of KwaZulu-Natal" },
            new University { UniversityName = "University of Johannesburg" },
            new University { UniversityName = "University of the Western Cape" },
            new University { UniversityName = "North-West University" },
            new University { UniversityName = "University of Limpopo" },
            new University { UniversityName = "University of the Free State" },
            new University { UniversityName = "University of Venda" },
            new University { UniversityName = "Walter Sisulu University" },
            new University { UniversityName = "Tshwane University of Technology" },
            new University { UniversityName = "Cape Peninsula University of Technology" },
            new University { UniversityName = "Durban University of Technology" }
                };

                // Add the universities to the context and save changes
                context.universities.AddRange(universities);
                context.SaveChanges();
            }
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


        //Subjects
        private static void SeedSubjects(ApplicationDbContext context)
        {
            var listofSubjects = new Subject[]
            {
        new Subject { Name = "English Home Language" },//1
        new Subject { Name = "English First Additional Language" },//2
        new Subject { Name = "Mathematics" },//3
        new Subject { Name = "Mathematical Literacy" },//4
        new Subject { Name = "IsiZulu Home Language" },//5
        new Subject { Name = "IsiZulu First Additional Language" },//6
        new Subject { Name = "Afrikaans Home Language" },//7
        new Subject { Name = "Afrikaans First Additional Language" },//8
        new Subject { Name = "Life Orientation" },//9
        new Subject { Name = "Physical Science" },//10
        new Subject { Name = "Life Sciences" },//11
        new Subject { Name = "Geography" },//12
        new Subject { Name = "History" },//13
        new Subject { Name = "Accounting" },//14
        new Subject { Name = "Business Studies" },//15
        new Subject { Name = "Economics" },//16
        new Subject { Name = "Information Technology" },//17
        new Subject { Name = "Computer Applications Technology" },//18
        new Subject { Name = "Agricultural Science" },//19
        new Subject { Name = "Agricultural Technology" },//20
        new Subject { Name = "Agricultural Management Practices" },//21
        new Subject { Name = "Tourism" },//22
        new Subject { Name = "Hospitality Studies" },//23
        new Subject { Name = "Consumer Studies" },//24
        new Subject { Name = "Engineering Graphics and Design" },//25
        new Subject { Name = "Visual Arts" },//26
        new Subject { Name = "Dance Studies" },//27
        new Subject { Name = "Design" },//28
        new Subject { Name = "Dramatic Arts" },//29
        new Subject { Name = "Music" },//30
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

        //university with their courses and required points
        private static void SeedCourses(ApplicationDbContext context)
        {
            var courses = new Course[]
            {
                new Course { University = "University of KwaZulu-Natal", CourseName = "B Sc Computer Science & Information Technology", Points = 34 },//C ID 1
                new Course { University = "Durban University of Technology", CourseName = "Bachelor of Information and Communications Technology", Points = 30 }, //C ID 2
                new Course { University = "Durban University of Technology", CourseName = "Bachelor of Information and Communications Technology in Internet of Things (IoT)", Points = 28 },// C ID 3
                new Course { University = "Durban University of Technology", CourseName = "Dip ICT: Applications Development", Points = 26 }, //C ID 4
                new Course { University = "Durban University of Technology", CourseName = "Dip ICT: Applications Development (4 year Foundation programme)", Points = 26 }, //C ID 5
                new Course { University = "Durban University of Technology", CourseName = "Dip ICT: Business Analysis", Points = 26 }, //C ID 6
           
                new Course{University="Mangosuthu University of Technology", CourseName="Diploma in Information Technology", Points=24}, //C ID 7
                new Course { University = "Mangosuthu University of Technology", CourseName = "Diploma in Information Technology (ECP)", Points = 23 },//C ID 8
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
                new SubjectRequired { SubjectId = 3, CourseId = 1, SubjectLevel = 5, /*SubjectName = "Mathematics"*/ },
                new SubjectRequired{SubjectId=9, CourseId=1, SubjectLevel=4,/*SubjectName="Life Orientation"*/ },

               // Required subjects for Bachelor of Information and Communications Technology at DUT
               new SubjectRequired { SubjectId = 3, CourseId = 2, SubjectLevel = 4,/*SubjectName = "Mathematics"*/ },
               new SubjectRequired{SubjectId=9, CourseId=2, SubjectLevel=4, /*SubjectName="Life Orientation"*/ },
                       
               //BACHELOR OF INFORMATION AND COMMUNICATION TECHNOLOGY IN IOT
               new SubjectRequired { SubjectId = 3, CourseId = 3, SubjectLevel = 4, /*SubjectName="Mathematics"*/ },
               new SubjectRequired{SubjectId=9, CourseId=3, SubjectLevel=4, /*SubjectName="Life Orientation"*/ },
               
               // Required subjects for Dip ICT: Applications Development at DUT
               new SubjectRequired { SubjectId = 9, CourseId = 4, SubjectLevel = 4,/*SubjectName="Life Orientation"*/ }, 
               
               // Required subjects for Dip ICT: Applications Development (4 year Foundation programme) at DUT 
               new SubjectRequired { SubjectId = 9, CourseId = 5, SubjectLevel = 4, /*SubjectName="Life Orientation"*/ }, 
               
               // Required subjects for Dip ICT: Business Analysis at DUT
               new SubjectRequired { SubjectId = 9, CourseId = 6, SubjectLevel = 4,/*SubjectName= "Life Orientation"*/ }, 
               
               // Required subjects for Diploma in Information Technology at MUT
               new SubjectRequired{SubjectId=9, CourseId=7, SubjectLevel=4, /*SubjectName="Life Orientation"*/},
               
               // Required subjects for Diploma in Information Technology (ECP) at MUT 
               new SubjectRequired { SubjectId = 9, CourseId = 8, SubjectLevel = 4 , /*SubjectName="Life Orientation"*/},
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
                new AlternativeSubject { SubjectId = 1, CourseId = 4, AlternativeSubjectLevel = 3, AlternativeSubjectName = "English Home Language" },
                new AlternativeSubject { SubjectId = 2, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
                new AlternativeSubject {SubjectId=3, CourseId=4, AlternativeSubjectLevel=3, AlternativeSubjectName="Mathematic"},
                new AlternativeSubject {SubjectId=4, CourseId=4, AlternativeSubjectLevel=6, AlternativeSubjectName="Mathematic literacy"},
                new AlternativeSubject { SubjectId = 19, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
                new AlternativeSubject { SubjectId = 11, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
                new AlternativeSubject { SubjectId = 10, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
                new AlternativeSubject { SubjectId = 3, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Geography" },
                new AlternativeSubject { SubjectId = 4, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "History" },
                new AlternativeSubject { SubjectId = 5, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Accounting" },
                new AlternativeSubject { SubjectId = 6, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Business Studies" },
                new AlternativeSubject { SubjectId = 7, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Economics" },
                new AlternativeSubject { SubjectId = 8, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Information Technology" },
                new AlternativeSubject { SubjectId = 9, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Computer Applications Technology" },
                new AlternativeSubject { SubjectId = 12, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Technology" },
                new AlternativeSubject { SubjectId = 13, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Management Practices" },
                new AlternativeSubject { SubjectId = 14, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Tourism" },
                new AlternativeSubject { SubjectId = 15, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Hospitality Studies" },
                new AlternativeSubject { SubjectId = 16, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Consumer Studies" },
                new AlternativeSubject { SubjectId = 17, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Engineering Graphics and Design" },
                new AlternativeSubject { SubjectId = 18, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Visual Arts" },
                new AlternativeSubject { SubjectId = 20, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dance Studies" },
                new AlternativeSubject { SubjectId = 21, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Design" },
                new AlternativeSubject { SubjectId = 22, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dramatic Arts" },
                new AlternativeSubject { SubjectId = 23, CourseId = 4, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Music" },
                
                // Alternative subjects for Dip ICT: Applications Development (4 year Foundation programme) at DUT
                new AlternativeSubject { SubjectId = 1, CourseId = 5, AlternativeSubjectLevel = 3, AlternativeSubjectName = "English Home Language" },
                new AlternativeSubject { SubjectId = 2, CourseId = 5, AlternativeSubjectLevel = 3, AlternativeSubjectName = "English First Additional Language" },
                new AlternativeSubject {SubjectId=3, CourseId=5, AlternativeSubjectLevel=3, AlternativeSubjectName="Mathematic"},
                new AlternativeSubject {SubjectId=4, CourseId=5, AlternativeSubjectLevel=5, AlternativeSubjectName="Mathematic literacy"},
                new AlternativeSubject { SubjectId = 19, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
                new AlternativeSubject { SubjectId = 11, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
                new AlternativeSubject { SubjectId = 10, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
                new AlternativeSubject { SubjectId = 3, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Geography" },
                new AlternativeSubject { SubjectId = 4, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "History" },
                new AlternativeSubject { SubjectId = 5, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Accounting" },
                new AlternativeSubject { SubjectId = 6, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Business Studies" },
                new AlternativeSubject { SubjectId = 7, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Economics" },
                new AlternativeSubject { SubjectId = 8, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Information Technology" },
                new AlternativeSubject { SubjectId = 9, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Computer Applications Technology" },
                new AlternativeSubject { SubjectId = 12, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Technology" },
                new AlternativeSubject { SubjectId = 13, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Management Practices" },
                new AlternativeSubject { SubjectId = 14, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Tourism" },
                new AlternativeSubject { SubjectId = 15, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Hospitality Studies" },
                new AlternativeSubject { SubjectId = 16, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Consumer Studies" },
                new AlternativeSubject { SubjectId = 17, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Engineering Graphics and Design" },
                new AlternativeSubject { SubjectId = 18, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Visual Arts" },
                new AlternativeSubject { SubjectId = 20, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dance Studies" },
                new AlternativeSubject { SubjectId = 21, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Design" },
                new AlternativeSubject { SubjectId = 22, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dramatic Arts" },
                new AlternativeSubject { SubjectId = 23, CourseId = 5, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Music" },
                
                
                // Alternative subjects for Dip ICT: Business Analysis at DUT
                new AlternativeSubject { SubjectId = 1, CourseId = 6, AlternativeSubjectLevel = 3, AlternativeSubjectName = "English Home Language" },
                new AlternativeSubject { SubjectId = 2, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
                new AlternativeSubject { SubjectId = 3, CourseId = 6, AlternativeSubjectLevel = 3, AlternativeSubjectName = "Mathematics" },
                new AlternativeSubject { SubjectId = 4, CourseId = 6, AlternativeSubjectLevel = 6, AlternativeSubjectName = "Mathematical Literacy" },
                new AlternativeSubject { SubjectId = 19, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
                new AlternativeSubject { SubjectId = 11, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
                new AlternativeSubject { SubjectId = 10, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
                new AlternativeSubject { SubjectId = 3, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Geography" },
                new AlternativeSubject { SubjectId = 4, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "History" },
                new AlternativeSubject { SubjectId = 5, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Accounting" },
                new AlternativeSubject { SubjectId = 6, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Business Studies" },
                new AlternativeSubject { SubjectId = 7, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Economics" },
                new AlternativeSubject { SubjectId = 8, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Information Technology" },
                new AlternativeSubject { SubjectId = 9, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Computer Applications Technology" },
                new AlternativeSubject { SubjectId = 12, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Technology" },
                new AlternativeSubject { SubjectId = 13, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Management Practices" },
                new AlternativeSubject { SubjectId = 14, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Tourism" },
                new AlternativeSubject { SubjectId = 15, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Hospitality Studies" },
                new AlternativeSubject { SubjectId = 16, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Consumer Studies" },
                new AlternativeSubject { SubjectId = 17, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Engineering Graphics and Design" },
                new AlternativeSubject { SubjectId = 18, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Visual Arts" },
                new AlternativeSubject { SubjectId = 20, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dance Studies" },
                new AlternativeSubject { SubjectId = 21, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Design" },
                new AlternativeSubject { SubjectId = 22, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Dramatic Arts" },
                new AlternativeSubject { SubjectId = 23, CourseId = 6, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Music" },

                new AlternativeSubject { SubjectId = 1, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
                new AlternativeSubject { SubjectId = 2, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
                new AlternativeSubject{SubjectId=3, CourseId=7, AlternativeSubjectLevel=3, AlternativeSubjectName="Mathematic"},
                new AlternativeSubject{SubjectId=4, CourseId=7, AlternativeSubjectLevel=5, AlternativeSubjectName="Mathematic literacy"},
                
                
                // Alternative subjects for Diploma in Information Technology at MUT
                new AlternativeSubject { SubjectId = 1, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
                new AlternativeSubject { SubjectId = 2, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
                new AlternativeSubject { SubjectId = 10, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Physical Science" },
                new AlternativeSubject { SubjectId = 11, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Life Sciences" },
                new AlternativeSubject { SubjectId = 19, CourseId = 7, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Agricultural Science" },
                
                // Alternative subjects for Diploma in Information Technology (ECP) at MUT
                new AlternativeSubject { SubjectId = 1, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English Home Language" },
                new AlternativeSubject { SubjectId = 2, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "English First Additional Language" },
                new AlternativeSubject { SubjectId = 3, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Mathematic" },
                new AlternativeSubject { SubjectId = 4, CourseId = 8, AlternativeSubjectLevel = 4, AlternativeSubjectName = "Mathematic literacy" },
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
    }
}
