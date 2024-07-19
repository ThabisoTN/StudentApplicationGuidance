
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using StudentApplicationGuidance.ModelView;
using StudentApplicationGuidance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentApplicationGuidance.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CourseQualificationService _qualificationService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ApplicationDbContext context, CourseQualificationService qualificationService, ILogger<CoursesController> logger)
        {
            _context = context;
            _qualificationService = qualificationService;
            _logger = logger;
        }



        [HttpGet]
        public async Task<IActionResult> SelectCourse()
        {
            _logger.LogInformation("SelectCourse action method called");

            var universities = await _context.Courses.Select(c => c.University).Distinct().ToListAsync();
            var courses = await _context.Courses.Select(c => c.CourseName).Distinct().ToListAsync();

            var modelView = new SelectCourseViewModel
            {
                Universities = universities,
                Courses = courses
            };

            return View(modelView);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursesByUniversity(string university)
        {
            if (string.IsNullOrEmpty(university))
            {
                return Json(new List<string>());
            }

            var courses = await _context.Courses
                .Where(c => c.University == university)
                .Select(c => c.CourseName)
                .ToListAsync();

            return Json(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CheckQualification(string university, string courseName)
        {
            if (string.IsNullOrEmpty(university) || string.IsNullOrEmpty(courseName))
            {
                return Json(new { success = false, message = "University and course must be selected." });
            }

            var course = await _context.Courses.Include(c => c.SubjectRequired).ThenInclude(sr => sr.Subject).Include(c => c.AlternativeSubjects).ThenInclude(asub => asub.Subject).FirstOrDefaultAsync(c => c.University == university && c.CourseName == courseName);

            if (course == null)
            {
                return Json(new { success = false, message = "Course not found." });
            }

            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not logged in, login required." });
            }

            try
            {
               
                var userSubjects = await _context.UserSubjects
                    .Where(us => us.UserId == userId)
                    .Include(us => us.Subject)
                    .ToListAsync();

                if (userSubjects == null || !userSubjects.Any())
                {
                    return Json(new { success = false, message = "User does not have subjects. Please save subjects to the system." });
                }

                var (qualifies, reasons) = _qualificationService.CheckCourseQualification(course, userSubjects);

                string message = qualifies
                    ? "Congratulations! You qualify for the course based on your selected subjects and levels. You can now proceed to apply for this course at your chosen university. Best of luck with your application!"
                    : $"You do not meet the minimum requirements for this course. Reasons:\n{string.Join("\n", reasons)}";

                return Json(new { success = qualifies, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking qualification");
                return Json(new { success = false, message = $"Error checking qualification: {ex.Message}" });
            }
        }
    }
}

