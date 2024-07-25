using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentApplicationGuidance.Services
{
    public class CourseQualificationService
    {
        public (bool Qualifies, List<string> Reasons) CheckCourseQualification(Course course, List<UserSubject> userSubjects)
        {
            var reasons = new List<string>();
            var requiredSubjects = course.SubjectRequired.ToList();
            var alternativeSubjects = course.AlternativeSubjects.ToList();

            // Check required subjects
            foreach (var requiredSubject in requiredSubjects)
            {
                var userSubject = userSubjects.FirstOrDefault(us => us.SubjectId == requiredSubject.SubjectId);
                

                if (userSubject == null || userSubject.Level < requiredSubject.SubjectLevel)
                {
                    reasons.Add($"You do not have required subject or Your level for {requiredSubject.Subject.Name} is bellow required level for this course,  Required level is: {requiredSubject.SubjectLevel},");
                }
            
            }
            // Check if the user has English Home Language or First Additional Language at Level 4
            //bool hasEnglish = userSubjects.Any(us =>(us.Subject.Name == "English Home Language" || us.Subject.Name == "English First Additional Language") && us.Level >= 4);
            //if (!hasEnglish)
            //{
            //    reasons.Add("You do not have English (Home Language or First Additional Language) at Level 4.");
            //}

            // Checking if user has any of the altenative subjects. U
            if (alternativeSubjects.Any())
            {
                bool hasAlternative = alternativeSubjects.Any(altSub =>userSubjects.Any(us => us.SubjectId == altSub.SubjectId && us.Level >= altSub.AlternativeSubjectLevel));
                if (!hasAlternative)
                {
                    reasons.Add("You do not have any of the required alternative subject");
                }
            }

            // Calculating user total subject points 
            int totalPoints = CalculateTotalPoints(userSubjects);
            if (totalPoints < course.Points)
            {
                reasons.Add($"Your total  points does not meet required number of points. Required: {course.Points}, while you achieved a total of : {totalPoints} points excluding LO.");
            }

            return (reasons.Count == 0, reasons);
        }

        private int CalculateTotalPoints(List<UserSubject> userSubjects)
        {
            return userSubjects.Where(us => us.Level > 1 && us.Subject.Name != "Life Orientation").Sum(us => us.Level);
        }
    }
}
