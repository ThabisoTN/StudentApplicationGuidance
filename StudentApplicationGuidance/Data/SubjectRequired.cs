using StudentApplicationGuidance.Models;

namespace StudentApplicationGuidance.Data
{
    public class SubjectRequired
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int SubjectLevel { get; set; }
    }
}
