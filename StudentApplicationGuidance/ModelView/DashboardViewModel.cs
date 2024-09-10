namespace StudentApplicationGuidance.ModelView
{
    public class DashboardViewModel
    {
        public int UserCount { get; set; }
        public int CourseCount { get; set; }
        public List<dynamic> CoursesByUniversity { get; set; }
        public List<dynamic> UsersByProvince { get; set; }
    }
}
