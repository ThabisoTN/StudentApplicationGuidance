using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using StudentApplicationGuidance.ModelView;
using StudentApplicationGuidance.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentApplicationGuidance.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TutorAIService _tutorAIService;
        private readonly CourseQualificationService _qualificationService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(
            ApplicationDbContext context,
            TutorAIService tutorAIService,
            CourseQualificationService qualificationService,
            ILogger<CoursesController> logger)
        {
            _context = context;
            _tutorAIService = tutorAIService;
            _qualificationService = qualificationService;
            _logger = logger;
        }

        public IActionResult Index(string university)
        {
            var coursesQuery = _context.Courses
                                       .AsNoTracking() // For performance, disable tracking as we are not modifying entities
                                       .Include(c => c.University) // Ensure University is eagerly loaded
                                       .Include(c => c.SubjectRequired).ThenInclude(sr => sr.Subject)
                                       .Include(c => c.AlternativeSubjects).ThenInclude(asub => asub.Subject)
                                       .AsQueryable();

            if (!string.IsNullOrEmpty(university))
            {
                coursesQuery = coursesQuery.Where(c => c.University.UniversityName == university);
            }

            var courses = coursesQuery.ToList();

            var model = new CourseListViewModel
            {
                Courses = courses,
                Universities = _context.SAUniversities.Select(u => u.UniversityName).Distinct().ToList(),
                SelectedUniversity = university,
                UserSubjects = _context.UserSubjects
                                        .Include(us => us.Subject)
                                        .Where(us => us.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                        .ToList()
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CourseCards", model.Courses);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SelectCourse()
        {
            _logger.LogInformation("SelectCourse action method called");

            var universities = await _context.SAUniversities.Select(u => u.UniversityName).Distinct().ToListAsync();
            var courses = await _context.Courses.Select(c => c.CourseName).Distinct().ToListAsync();

            var modelView = new SelectCourseViewModel
            {
                Universities = universities,
                Courses = courses
            };

            return View(modelView);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursesByUniversity(string universityName)
        {
            if (string.IsNullOrEmpty(universityName))
            {
                return Json(new List<string>());
            }

            var universityId = await _context.SAUniversities
                                             .Where(u => u.UniversityName == universityName)
                                             .Select(u => u.Id)
                                             .FirstOrDefaultAsync();

            if (universityId == 0)
            {
                return Json(new List<string>());
            }

            var courses = await _context.Courses
                                        .Where(c => c.UniversityId == universityId)
                                        .Select(c => c.CourseName)
                                        .ToListAsync();

            return Json(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CheckQualificationAjax(string university, string courseName)
        {
            try
            {
                if (string.IsNullOrEmpty(university) || string.IsNullOrEmpty(courseName))
                {
                    return Json(new { success = false, message = "University and course must be selected." });
                }

                var course = await _context.Courses
                    .Include(c => c.University)
                    .Include(c => c.SubjectRequired).ThenInclude(sr => sr.Subject)
                    .Include(c => c.AlternativeSubjects).ThenInclude(asub => asub.Subject)
                    .FirstOrDefaultAsync(c => c.University.UniversityName == university && c.CourseName == courseName);

                if (course == null)
                {
                    return Json(new { success = false, message = "Course not found." });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Please log in to check qualification." });
                }

                var userSubjects = await _context.UserSubjects
                                                 .Include(us => us.Subject)
                                                 .Where(us => us.UserId == userId)
                                                 .ToListAsync();

                if (!userSubjects.Any())
                {
                    return Json(new { success = false, message = "Please add your subjects to check qualification." });
                }

                var (qualifies, _) = _qualificationService.CheckCourseQualification(course, userSubjects);

                string message = qualifies
                    ? "You qualify for this course."
                    : "You do not meet all requirements for this course.";

                return Json(new { success = qualifies, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking qualification for university: {University}, course: {CourseName}", university, courseName);
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckQualification(string universityName, string courseName)
        {
            if (string.IsNullOrEmpty(universityName) || string.IsNullOrEmpty(courseName))
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var universityId = await _context.SAUniversities
                                             .Where(u => u.UniversityName == universityName)
                                             .Select(u => u.Id)
                                             .FirstOrDefaultAsync();

            if (universityId == 0)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var course = await _context.Courses
                                       .Include(c => c.SubjectRequired)
                                       .ThenInclude(sr => sr.Subject)
                                       .Include(c => c.AlternativeSubjects)
                                       .ThenInclude(asub => asub.Subject)
                                       .FirstOrDefaultAsync(c => c.UniversityId == universityId && c.CourseName == courseName);

            if (course == null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userSubjects = await _context.UserSubjects
                                             .Include(us => us.Subject)
                                             .Where(us => us.UserId == userId)
                                             .ToListAsync();

            var (qualifies, reasons) = _qualificationService.CheckCourseQualification(course, userSubjects);

            var viewModel = new QualificationResultViewModel
            {
                Course = course,
                Qualifies = qualifies,
                Reasons = reasons,
                UserSubjects = userSubjects
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCareerAdvice([FromQuery] int courseId)
        {
            try
            {
                var course = await _context.Courses
                    .Include(c => c.SubjectRequired)
                    .ThenInclude(sr => sr.Subject)
                    .Include(c => c.AlternativeSubjects)
                    .ThenInclude(asub => asub.Subject)
                    .FirstOrDefaultAsync(c => c.CourseId == courseId);

                if (course == null)
                {
                    return Json(new { success = false, message = "Course not found." });
                }

                var courses = new List<Course> { course };
                var careerAdvice = await _tutorAIService.GenerateCareerAdviceAsync(courses);
                return Json(new { success = true, advice = careerAdvice });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating career advice for courseId: {CourseId}", courseId);
                return Json(new { success = false, message = "An error occurred while generating career advice. Please try again." });
            }
        }
    }
}
