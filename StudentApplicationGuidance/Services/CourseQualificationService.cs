
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using System.Collections.Generic;
using System.Linq;

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
                reasons.Add($"You do not have the required subject or your level for {requiredSubject.Subject.Name} is below the required level for this course. Required level is: {requiredSubject.SubjectLevel}.");
            }
        }

        // Define a list of subjects to exclude from the count
        var excludedSubjects = new List<string>
        {
            "English Home Language",
            "English First Additional Language",
            "Life Orientation",
            "Mathematics",
            "Mathematical Literacy"
        };

        // Dynamic exclusion logic for IsiZulu and Afrikaans
        bool isiZuluCounted = userSubjects.Any(us => us.Subject.Name.StartsWith("IsiZulu") && us.Level > 1);
        bool afrikaansCounted = userSubjects.Any(us => us.Subject.Name.StartsWith("Afrikaans") && us.Level > 1);

        if (isiZuluCounted)
        {
            excludedSubjects.Add("Afrikaans Home Language");
            excludedSubjects.Add("Afrikaans First Additional Language");
        }
        else if (afrikaansCounted)
        {
            excludedSubjects.Add("IsiZulu Home Language");
            excludedSubjects.Add("IsiZulu First Additional Language");
        }

        // Filter out the excluded subjects from alternative subjects before counting
        var filteredAlternativeSubjects = alternativeSubjects.Where(altSub => !excludedSubjects.Contains(altSub.Subject.Name)).ToList();

        // Check if user meets the minimum number of alternative subjects
        var qualifiedAlternativeSubjects = filteredAlternativeSubjects.Where(altSub => userSubjects.Any(us => us.SubjectId == altSub.SubjectId && us.Level >= altSub.AlternativeSubjectLevel)).Count();

        if (qualifiedAlternativeSubjects < filteredAlternativeSubjects.FirstOrDefault()?.NumberOfRequiredAlternativeSubjects)
        {
            reasons.Add($"You do not meet the minimum number of required alternative subjects. You need at least {filteredAlternativeSubjects.First().NumberOfRequiredAlternativeSubjects} alternative subjects.");
        }


        // Calculating user total subject points
        int totalPoints = CalculateTotalPoints(userSubjects);
        if (totalPoints < course.Points)
        {
            reasons.Add($"Your total points do not meet the required number of points. Required: {course.Points}, while you achieved a total of: {totalPoints} points excluding Life Orientation.");
        }

        return (reasons.Count == 0, reasons);
    }

    private int CalculateTotalPoints(List<UserSubject> userSubjects)
    {
        return userSubjects.Where(us => us.Level > 1 && us.Subject.Name != "Life Orientation").Sum(us => us.Level);
    }
}
 