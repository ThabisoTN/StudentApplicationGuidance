using StudentApplicationGuidance.Models;

namespace StudentApplicationGuidance.Data
{
    public class AlternativeSubject
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int NumberOfRequiredAlternativeSubjects { get; set; }
        public Subject Subject { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string AlternativeSubjectName { get; set; }
        public int AlternativeSubjectLevel { get; set; }
    }
}
