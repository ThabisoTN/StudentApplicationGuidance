using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
namespace StudentApplicationGuidance.Data
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public virtual ICollection<UserSubject> UserSubjects { get; set; }
        public virtual ICollection<SubjectRequired> SubjectRequired { get; set; }
        public virtual ICollection<AlternativeSubject> AlternativeSubjects { get; set; }

    }
}
