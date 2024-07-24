using StudentApplicationGuidance.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentApplicationGuidance.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string University { get; set; }
        public string CourseName { get; set; }
        public int Points { get; set; }
    

        public virtual ICollection<SubjectRequired> SubjectRequired { get; set; }
        public virtual ICollection<AlternativeSubject> AlternativeSubjects { get; set; }

    }
}
