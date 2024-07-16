namespace StudentApplicationGuidance.ModelView
{
    public class EnterCourseViewModel
    {
        public string CourseName { get; set; }
        public bool IsLoading { get; set; }
        public string Message { get; set; }
        public bool IsEligible { get; set; }
        public Dictionary<string, Dictionary<string, int[]>> CourseRequirements { get; set; }
    }
}
