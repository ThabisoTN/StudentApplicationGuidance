using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.ModelView;
using StudentApplicationGuidance.Models;

namespace StudentApplicationGuidance.Services
{
    public class UserSubjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSubjectService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Subject>> GetAllSubjects()
        {
            return await _context.Subjects.ToListAsync();
        }

        public async Task AddUserSubjectsAsync(string userId, List<int> subjectIds, List<int> levels)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var userSubjects = subjectIds.Select((id, index) => new UserSubject
            {
                UserId = userId,
                SubjectId = id,
                Level = levels[index]
            }).ToList();

            _context.UserSubjects.AddRange(userSubjects);
            await _context.SaveChangesAsync();
        }


        public async Task<int> CalculatePoints(string userId)
        {
            var userSubjects = await _context.UserSubjects.Where(us => us.UserId == userId).Include(us => us.Subject).ToListAsync();

            int totalPoints = userSubjects
                .Where(us => us.Level > 1 && us.Subject.Name != "Life Orientation")
                .Sum(us => us.Level);

            return totalPoints;
        }

        public async Task<List<UserSubjectView>> GetUserSubjects(string userId)
        {
            var userSubjects = await _context.UserSubjects.Where(us => us.UserId == userId).Include(us => us.Subject).ToListAsync();

            var userSubjectViews = userSubjects.Select(us => new UserSubjectView
            {
                UserSubjectId = us.Id,
                SubjectId = us.Subject.Id,
                SubjectName = us.Subject.Name,
                UserId = us.UserId,
                UserName = us.User.UserName, // Assuming `UserName` is the desired property
                Level = us.Level
            }).ToList();

            return userSubjectViews;
        }
    }
}
