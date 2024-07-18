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
                    reasons.Add($"Missing or insufficient level for {requiredSubject.Subject.Name}. Required level: {requiredSubject.SubjectLevel}");
                }
            }

            // Check alternative subjects
            //if (alternativeSubjects.Any())
            //{
            //    bool hasAlternative = alternativeSubjects.Any(altSub =>
            //        userSubjects.Any(us => us.SubjectId == altSub.SubjectId && us.Level >= altSub.AlternativeSubjectLevel));
            //    if (!hasAlternative)
            //    {
            //        reasons.Add("Missing required alternative subject");
            //    }
            //}

            // Calculate total points
            int totalPoints = CalculateTotalPoints(userSubjects);
            if (totalPoints < course.Points)
            {
                reasons.Add($"Insufficient points. Required: {course.Points}, Achieved: {totalPoints}");
            }

            return (reasons.Count == 0, reasons);
        }

        private int CalculateTotalPoints(List<UserSubject> userSubjects)
        {
            return userSubjects
                .Where(us => us.Level > 1 && us.Subject.Name != "Life Orientation")
                .Sum(us => us.Level);
        }
    }
}
