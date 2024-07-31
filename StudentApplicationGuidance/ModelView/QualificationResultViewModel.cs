using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;

namespace StudentApplicationGuidance.ModelView
{
    public class QualificationResultViewModel
    {
        public Course Course { get; set; }
        public bool Qualifies { get; set; }
        public List<string> Reasons { get; set; }
        public List<UserSubject> UserSubjects { get; set; }
        public List<UserSubject> NonQualifyingSubjects { get; set; }
        public List<Subject> QualifyingSubjects { get; set; }


    }
}
