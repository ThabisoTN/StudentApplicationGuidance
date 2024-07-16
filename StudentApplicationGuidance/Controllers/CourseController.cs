using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentApplicationGuidance.ModelView;


namespace YourNamespace.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult EnterCourseName()
        {
            var viewModel = new EnterCourseViewModel
            {
                CourseRequirements = GetCourseRequirements()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EnterCourseName(EnterCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                var (eligible, reason) = await CheckCourseEligibility(model.CourseName, userId);

                model.IsEligible = eligible;
                model.Message = eligible ? $"Congratulations! You are eligible for the {model.CourseName} program." : $"Sorry, you do not meet the requirements for the {model.CourseName} program. Details: {reason}";
            }

            model.CourseRequirements = GetCourseRequirements();
            return View(model);
        }

        private Dictionary<string, Dictionary<string, int[]>> GetCourseRequirements()
        {
            return new Dictionary<string, Dictionary<string, int[]>>
            {
                {
                    "BSc Computer Science & Information Technology",
                    new Dictionary<string, int[]>
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
                    "Bachelor of Information and Communications Technology (ICT)",
                    new Dictionary<string, int[]>
                    {
                        { "Mathematics", new int[] { 4 } },
                        { "English Home Language", new int[] { 4 } },
                    }
                },
                {
                    "Bachelor of Information and Communications Technology in Internet of Things (IoT)",
                    new Dictionary<string, int[]>
                    {
                        { "Mathematics", new int[] { 4 } },
                        { "English Home Language", new int[] { 4 } },
                    }
                },
                {
                    "Diploma in Information and Communication Technology (ICT): Applications Development",
                    new Dictionary<string, int[]>
                    {
                        { "English Home Language", new int[] { 3, 4 } },
                        { "Mathematics", new int[] { 3, 6 } },
                    }
                },
            };
        }

        private async Task<(bool isEligible, string reason)> CheckCourseEligibility(string selectedCourse, string userId)
        {
            if (!GetCourseRequirements().TryGetValue(selectedCourse, out var courseRequirement))
            {
                return (false, $"Course requirements not found for {selectedCourse}.");
            }

            var userSubjectsAndLevels = await GetUserSubjectsAndLevels(userId);
            var unmetRequirements = new List<string>();

            foreach (var requirement in courseRequirement)
            {
                string subjectName = requirement.Key;
                int[] requiredLevels = requirement.Value;

                if (!userSubjectsAndLevels.ContainsKey(subjectName))
                {
                    unmetRequirements.Add($"You do not have the required subject: {subjectName}");
                    continue;
                }

                int userLevel = userSubjectsAndLevels[subjectName];
                bool meetsRequirement = requiredLevels.Any(level => userLevel >= level);

                if (!meetsRequirement)
                {
                    unmetRequirements.Add($"You do not meet minimum requirement level in {subjectName}: required {string.Join(" or ", requiredLevels)}, but you got {userLevel}");
                }
            }

            if (unmetRequirements.Any())
            {
                return (false, string.Join("; ", unmetRequirements));
            }

            return (true, "User meets all requirements.");
        }

        private async Task<Dictionary<string, int>> GetUserSubjectsAndLevels(string userId)
        {
            var userSubjects = await _context.UserSubjects
                                             .Where(us => us.User.Id == userId)
                                             .Include(us => us.Subject)
                                             .ToListAsync();

            var subjectLevels = new Dictionary<string, int>();

            foreach (var userSubject in userSubjects)
            {
                if (userSubject.Subject != null)
                {
                    subjectLevels[userSubject.Subject.Name] = userSubject.Level;
                }
            }

            return subjectLevels;
        }
    }
}
