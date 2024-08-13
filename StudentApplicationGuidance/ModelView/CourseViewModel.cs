using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StudentApplicationGuidance.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
        public int SelectedUniversityId { get; set; }
        public IEnumerable<SelectListItem> Universities { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AllSubjects { get; set; } = new List<SelectListItem>();
        public IEnumerable<LevelOption> LevelOptions { get; set; } = new List<LevelOption>();
        public IEnumerable<int> SelectedRequiredSubjects { get; set; } = new List<int>();
        public IEnumerable<int> SelectedAlternativeSubjects { get; set; } = new List<int>();
        public int NumberOfRequiredAlternativeSubjects { get; set; }
        public Dictionary<int, int> RequiredSubjectLevels { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, int> AlternativeSubjectLevels { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, string> AlternativeSubjectNames { get; set; } = new Dictionary<int, string>();
    }



    public class LevelOption
    {
        public int Level { get; set; }
        public string Description { get; set; }
    }
}
