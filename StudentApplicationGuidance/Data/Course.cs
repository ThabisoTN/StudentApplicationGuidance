using StudentApplicationGuidance.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentApplicationGuidance.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public int UniversityId { get; set; }
        public virtual SAUniversities University { get; set; }
        public string CourseName { get; set; }
        public int Points { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public int NumberOfRequiredAlternativeSubjects { get; set; }


        public virtual ICollection<SubjectRequired> SubjectRequired { get; set; } = new List<SubjectRequired>();
        public virtual ICollection<AlternativeSubject> AlternativeSubjects { get; set; } = new List<AlternativeSubject>();
    }


}
