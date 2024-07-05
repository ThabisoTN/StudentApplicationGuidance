using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZizoAI.Models;

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

        // Retrieve all subjects asynchronously
        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        // Add subjects to a user with specified level
        public async Task AddUserSubjectsAsync(string userId, List<int> subjectIds, int level)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var userSubjects = subjectIds.Select(subjectId => new UserSubject
            {
                SubjectId = subjectId,
                User = user,
                Level = level
            }).ToList();

            _context.UserSubjects.AddRange(userSubjects);
            await _context.SaveChangesAsync();
        }

        // Add user subjects with a default level
        public async Task AddUserSubjectsWithDefaultLevelAsync(string userId, List<int> subjectIds)
        {
            await AddUserSubjectsAsync(userId, subjectIds, 1); // Assuming default level is 1
        }

        // Calculate total points for a user based on selected subjects
        public async Task<int> CalculatePointsAsync(string userId)
        {
            var userSubjects = await _context.UserSubjects
                                             .Where(us => us.User.Id == userId)
                                             .Include(us => us.Subject)
                                             .ToListAsync();

            int totalPoints = 0;
            foreach (var item in userSubjects)
            {
                if (item.Level > 1 && item.Subject.Name != "Life Orientation")
                {
                    totalPoints += item.Level;
                }
            }

            return totalPoints;
        }

        // Retrieve user subjects with related subject and user details
        public async Task<List<UserSubject>> GetUserSubjectsAsync(string userId)
        {
            var userSubjects = await _context.UserSubjects
                                             .Where(us => us.User.Id == userId)
                                             .Include(us => us.Subject)
                                             .Include(us => us.User)
                                             .ToListAsync();

            return userSubjects;
        }
    }
}
