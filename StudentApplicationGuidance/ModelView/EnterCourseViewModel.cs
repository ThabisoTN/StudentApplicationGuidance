using System.Collections.Generic;

namespace StudentApplicationGuidance.ModelView
{
    public class EnterCourseViewModel
    {
        public string SelectedUniversity { get; set; }
        public string SelectedFaculty { get; set; }
        public string SelectedDepartment { get; set; }
        public string SelectedCourse { get; set; }
        public bool IsEligible { get; set; }
        public string Message { get; set; }
        public bool IsLoading { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int[]>>>>> CourseRequirements { get; set; }
        public List<string> Reasons { get; set; }

        public EnterCourseViewModel()
        {
            CourseRequirements = GetCourseRequirements();
            Reasons = new List<string>();
        }

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int[]>>>>> GetCourseRequirements()
        {
            return new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int[]>>>>>
            {
                {
                    "DUT", new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int[]>>>>
                    {
                        {
                            "Faculty of Informatics and Accounting", new Dictionary<string, Dictionary<string, Dictionary<string, int[]>>>
                            {
                                {
                                    "Department of IT", new Dictionary<string, Dictionary<string, int[]>>
                                    {
                                        {
                                            "BSc Computer Science & Information Technology", new Dictionary<string, int[]>
                                            {
                                                { "Mathematics", new int[] { 5 } },
                                                { "English Home Language", new int[] { 4 } },
                                                { "Life Orientation", new int[] { 4 } },
                                                { "Agricultural Science", new int[] { 4 } },
                                                { "Life Science", new int[] { 4 } },
                                                { "Physical Science", new int[] { 4 } }
                                            }
                                        },
                                        {
                                            "Bachelor of Information and Communications Technology (ICT)", new Dictionary<string, int[]>
                                            {
                                                { "Mathematics", new int[] { 4 } },
                                                { "English Home Language", new int[] { 4 } }
                                            }
                                        },
                                        {
                                            "Bachelor of Information and Communications Technology in Internet of Things (IoT)", new Dictionary<string, int[]>
                                            {
                                                { "Mathematics", new int[] { 4 } },
                                                { "English Home Language", new int[] { 4 } }
                                            }
                                        },
                                        {
                                            "Diploma in Information and Communication Technology (ICT): Applications Development", new Dictionary<string, int[]>
                                            {
                                                { "English Home Language", new int[] { 3, 4 } },
                                                { "Mathematics", new int[] { 3, 6 } }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                {
                    "MUT", new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int[]>>>>
                    {
                        {
                            "Faculty of Engineering", new Dictionary<string, Dictionary<string, Dictionary<string, int[]>>>
                            {
                                {
                                    "Department of Electrical Engineering", new Dictionary<string, Dictionary<string, int[]>>
                                    {
                                        {
                                            "Diploma in Electrical Engineering", new Dictionary<string, int[]>
                                            {
                                                { "Mathematics", new int[] { 5 } },
                                                { "English Home Language", new int[] { 4 } }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
