using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using StudentApplicationGuidance.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly TutorAIService _tutorAIService;
    private readonly CourseQualificationService _qualificationService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(ApplicationDbContext context, TutorAIService tutorAIService, CourseQualificationService qualificationService, ILogger<AdminController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _tutorAIService = tutorAIService ?? throw new ArgumentNullException(nameof(tutorAIService));
        _qualificationService = qualificationService ?? throw new ArgumentNullException(nameof(qualificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IActionResult Index()
    {
        return View();
    }

    // GET: /Admin/ViewSubjects
    public async Task<IActionResult> ViewSubjects()
    {
        try
        {
            var subjects = await _context.Subjects.ToListAsync();
            return View(subjects);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving subjects: {ex.Message}");
            return View("Error");
        }
    }

    // Get: /Admin/ViewUsers
    public async Task<IActionResult> ApplicationUsers()
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving users: {ex.Message}");
            return View("Error");
        }
    }

    // Get: /Admin/Courses
    public async Task<IActionResult> Courses()
    {
        try
        {
            var course = await _context.Courses.ToListAsync();
            return View(course);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving Course: {ex.Message}");
            return View("Error");
        }
    }

    public IActionResult CreateCourse()
    {
        var viewModel = new CourseViewModel
        {
            Universities = _context.SAUniversities
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.UniversityName
                }).ToList(),

            AllSubjects = _context.Subjects
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList(),

            LevelOptions = Enumerable.Range(1, 7).Select(l => new LevelOption
            {
                Level = l,
                Description = $"Level {l}"
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCourse(CourseViewModel model)
    {
        _logger.LogInformation("CreateCourse POST method started.");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is not valid.");
            LogModelStateErrors();
            await PopulateViewModel(model);
            return View(model);
        }

        try
        {
            var university = await _context.SAUniversities.FindAsync(model.SelectedUniversityId);
            if (university == null)
            {
                ModelState.AddModelError("SelectedUniversityId", "Selected university not found.");
                _logger.LogWarning($"Selected university with ID {model.SelectedUniversityId} not found.");
                await PopulateViewModel(model);
                return View(model);
            }

            var course = new Course
            {
                UniversityId = model.SelectedUniversityId,
                CourseName = model.CourseName,
                Points = model.Points,
                Description = model.Description
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Course '{course.CourseName}' saved successfully with ID {course.CourseId}.");

            await SaveRequiredSubjects(model, course.CourseId);
            await SaveAlternativeSubjects(model, course.CourseId);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving the course and subjects.");
            ModelState.AddModelError("", "An error occurred while saving the course and subjects. Please try again.");
            await PopulateViewModel(model);
            return View(model);
        }
    }


    private async Task SaveRequiredSubjects(CourseViewModel model, int courseId)
    {
        if (model.SelectedRequiredSubjects != null && model.SelectedRequiredSubjects.Any())
        {
            foreach (var subjectId in model.SelectedRequiredSubjects)
            {
                var subjectLevel = model.RequiredSubjectLevels.TryGetValue(subjectId, out int level) ? level : 1;
                var subjectRequired = new SubjectRequired
                {
                    CourseId = courseId,
                    SubjectId = subjectId,
                    SubjectLevel = subjectLevel
                };
                _context.SubjectRequireds.Add(subjectRequired);
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Required subjects saved successfully for course ID {courseId}.");
        }
    }

    private async Task SaveAlternativeSubjects(CourseViewModel model, int courseId)
    {
        if (model.SelectedAlternativeSubjects != null && model.SelectedAlternativeSubjects.Any())
        {
            foreach (var subjectId in model.SelectedAlternativeSubjects)
            {
                var subjectLevel = model.AlternativeSubjectLevels.TryGetValue(subjectId, out int level) ? level : 1;

                // Fetch the subject name from the database using the SubjectId
                var subjectName = await _context.Subjects.Where(s => s.Id == subjectId).Select(s => s.Name).FirstOrDefaultAsync();

                var alternativeSubject = new AlternativeSubject
                {
                    CourseId = courseId,
                    SubjectId = subjectId,
                    AlternativeSubjectLevel = subjectLevel,
                    AlternativeSubjectName = subjectName,
                    NumberOfRequiredAlternativeSubjects = model.NumberOfRequiredAlternativeSubjects
                };

                _context.AlternativeSubjects.Add(alternativeSubject);
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Alternative subjects saved successfully for course ID {courseId}.");
        }
    }



    private void LogModelStateErrors()
    {
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                _logger.LogWarning($"Property: {state.Key}, Error: {error.ErrorMessage}");
            }
        }
    }





    private async Task PopulateViewModel(CourseViewModel model)
    {
        model.Universities = await _context.SAUniversities
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.UniversityName
            }).ToListAsync();

        model.AllSubjects = await _context.Subjects
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToListAsync();

        model.LevelOptions = Enumerable.Range(1, 7).Select(l => new LevelOption
        {
            Level = l,
            Description = $"Level {l}"
        }).ToList();
    }
}
