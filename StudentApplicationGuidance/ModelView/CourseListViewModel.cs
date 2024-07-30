using StudentApplicationGuidance.Models;
using System.Collections.Generic;

namespace StudentApplicationGuidance.ModelView
{
    public class CourseListViewModel
    {
        public List<Course> Courses { get; set; }
        public List<string> Universities { get; set; }
        public string SelectedUniversity { get; set; }
        public bool Qualifies { get; set; }
        public List<UserSubject> UserSubjects { get; set; } // Add this property
    }

}
