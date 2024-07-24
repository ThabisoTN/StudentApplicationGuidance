
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
        private readonly CourseQualificationService _qualificationService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ApplicationDbContext context, CourseQualificationService qualificationService, ILogger<CoursesController> logger)
        {
            _context = context;
            _qualificationService = qualificationService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string university)
        {
            var universities = await _context.Courses.Select(c => c.University).Distinct().OrderBy(u => u).ToListAsync();

            var coursesQuery = _context.Courses
                .Include(c => c.SubjectRequired)
                    .ThenInclude(sr => sr.Subject)
                .Include(c => c.AlternativeSubjects)
                    .ThenInclude(asub => asub.Subject)
                .AsQueryable();

            if (!string.IsNullOrEmpty(university))
            {
                coursesQuery = coursesQuery.Where(c => c.University == university);
            }

            var courses = await coursesQuery.ToListAsync();

            var viewModel = new CourseListViewModel
            {
                Courses = courses,
                Universities = universities,
                SelectedUniversity = university
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CourseCards", courses);
            }

            return View(viewModel);
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
        public async Task<IActionResult> CheckQualificationAjax(string university, string courseName)
        {
            try
            {
                if (string.IsNullOrEmpty(university) || string.IsNullOrEmpty(courseName))
                {
                    return Json(new { success = false, message = "University and course must be selected." });
                }

                var course = await _context.Courses
                    .Include(c => c.SubjectRequired)
                        .ThenInclude(sr => sr.Subject)
                    .Include(c => c.AlternativeSubjects)
                        .ThenInclude(asub => asub.Subject)
                    .FirstOrDefaultAsync(c => c.University == university && c.CourseName == courseName);

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
                    .Where(us => us.UserId == userId)
                    .Include(us => us.Subject)
                    .ToListAsync();

                if (userSubjects == null || !userSubjects.Any())
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
        public async Task<IActionResult> CheckQualification(string university, string courseName)
        {
            if (string.IsNullOrEmpty(university) || string.IsNullOrEmpty(courseName))
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var course = await _context.Courses
                .Include(c => c.SubjectRequired)
                    .ThenInclude(sr => sr.Subject)
                .Include(c => c.AlternativeSubjects)
                    .ThenInclude(asub => asub.Subject)
                .FirstOrDefaultAsync(c => c.University == university && c.CourseName == courseName);

            if (course == null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userSubjects = await _context.UserSubjects .Where(us => us.UserId == userId).Include(us => us.Subject).ToListAsync();

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


        //[HttpPost]
        //public async Task<IActionResult> CheckQualification(string university, string courseName)
        //{
        //    if (string.IsNullOrEmpty(university) || string.IsNullOrEmpty(courseName))
        //    {
        //        return Json(new { success = false, message = "University and course must be selected." });
        //    }

        //    var course = await _context.Courses.Include(c => c.SubjectRequired).ThenInclude(sr => sr.Subject).Include(c => c.AlternativeSubjects).ThenInclude(asub => asub.Subject).FirstOrDefaultAsync(c => c.University == university && c.CourseName == courseName);

        //    if (course == null)
        //    {
        //        return Json(new { success = false, message = "Course not found." });
        //    }

        //    var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Json(new { success = false, message = "User not logged in, login required." });
        //    }

        //    try
        //    {

        //        var userSubjects = await _context.UserSubjects
        //            .Where(us => us.UserId == userId)
        //            .Include(us => us.Subject)
        //            .ToListAsync();

        //        if (userSubjects == null || !userSubjects.Any())
        //        {
        //            return Json(new { success = false, message = "User does not have subjects. Please save subjects to the system." });
        //        }

        //        var (qualifies, reasons) = _qualificationService.CheckCourseQualification(course, userSubjects);

        //        string message = qualifies
        //            ? "You qualify for the course. You can apply for it at the selected university."
        //            : $"You do not meet the minimum requirements for this course. Reasons:\n{string.Join("\n", reasons)}";

        //        return Json(new { success = qualifies, message = message });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error checking qualification");
        //        return Json(new { success = false, message = $"Error checking qualification: {ex.Message}" });
        //    }
        //}
    }
}

