using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentApplicationGuidance.Models;
using System.Collections.Generic;

namespace StudentApplicationGuidance.ModelView
{
    public class CheckQualificationViewModel
    {
        public int CourseId { get; set; }
        public List<SelectListItem> Courses { get; set; }
        public List<SubjectSelection> Subjects { get; set; }
        public List<SelectListItem> AvailableSubjects { get; set; }
        public string Result { get; set; }
    }

    public class SubjectSelection
    {
        public int SubjectId { get; set; }
        public int Level { get; set; }
    }
}
