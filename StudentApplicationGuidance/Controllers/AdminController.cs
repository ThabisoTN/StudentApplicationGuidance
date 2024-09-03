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
    private int courseId;
    private int defaultLevel;

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
            _logger.LogError($"An error occurred while users: {ex.Message}");
            return View("Error");
        }
    }

    // Get: /Admin/Courses
    public async Task<IActionResult> Courses()
    {
        try
        {
            var courses = await _context.Courses.Include(c => c.University).ToListAsync();
            return View(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving Courses: {ex.Message}");
            return View("Error");
        }
    }


    public IActionResult CreateCourse()
    {
        var viewModel = new CourseViewModel
        {
            SAUniversities = _context.SAUniversities
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
    // GET: /Admin/EditCourse/5
    public async Task<IActionResult> EditCourse(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .Include(c => c.University)
            .Include(c => c.SubjectRequired)
            .Include(c => c.AlternativeSubjects)
            .FirstOrDefaultAsync(m => m.CourseId == id);

        if (course == null)
        {
            return NotFound();
        }

        var viewModel = new CourseViewModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            Points = course.Points,
            Description = course.Description,
            SelectedUniversityId = course.UniversityId,
            SAUniversities = _context.SAUniversities.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UniversityName }).ToList(),
            AllSubjects = _context.Subjects.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList(),
            LevelOptions = Enumerable.Range(1, 7).Select(i => new LevelOption { Level = i, Description = $"Level {i}" }).ToList(),
            // Ensure integer IDs are converted properly if required
            SelectedRequiredSubjects = course.SubjectRequired.Select(rs => rs.SubjectId).ToList(),  // Assuming SubjectId is an integer
            SelectedAlternativeSubjects = course.AlternativeSubjects.Select(subj => subj.SubjectId).ToList() // Assuming SubjectId is an integer
        };

        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCourse(int id, CourseViewModel model)
    {
        if (id != model.CourseId)
        {
            return NotFound();
        }

        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        List<string> changes = new List<string>();

        if (course.CourseName != model.CourseName)
        {
            changes.Add($"Course Name changed from '{course.CourseName}' to '{model.CourseName}'");
            course.CourseName = model.CourseName;
        }
        if (course.Description != model.Description)
        {
            changes.Add($"Description changed from '{course.Description}' to '{model.Description}'");
            course.Description = model.Description;
        }
        if (course.Points != model.Points)
        {
            changes.Add($"Points changed from '{course.Points}' to '{model.Points}'");
            course.Points = model.Points;
        }

        // Save the changes
        _context.Update(course);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Update Successful!";
        TempData["Changes"] = changes; // Storing changes in TempData

        return RedirectToAction(nameof(EditSuccess)); // Redirect to a confirmation page
    }

    public IActionResult EditSuccess()
    {
        return View();
    }



    private bool CourseExists(int id)
    {
        return _context.Courses.Any(c => c.CourseId == id);
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
        model.SAUniversities = await _context.SAUniversities
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

    private async Task UpdateRequiredSubjects(CourseViewModel model, int courseId)
    {
        // Fetch existing required subjects linked to this course
        var existingSubjects = _context.SubjectRequireds.Where(sr => sr.CourseId == courseId).ToList();
        var selectedSubjectIds = model.SelectedRequiredSubjects;

        // Add new required subjects that aren't already linked
        foreach (var subjectId in selectedSubjectIds)
        {
            if (!existingSubjects.Any(sr => sr.SubjectId == subjectId))
            {
                _context.SubjectRequireds.Add(new SubjectRequired { CourseId = courseId, SubjectId = subjectId });
            }
        }

        // Remove existing links that are not in the newly selected subjects
        _context.SubjectRequireds.RemoveRange(existingSubjects.Where(sr => !selectedSubjectIds.Contains(sr.SubjectId)));

        await _context.SaveChangesAsync();
    }

    private async Task UpdateAlternativeSubjects(CourseViewModel model, int courseId)
    {
        var existingAlternatives = _context.AlternativeSubjects.Where(a => a.CourseId == courseId).ToList();

        foreach (var subjectId in model.SelectedAlternativeSubjects)
        {
            var alternativeSubject = existingAlternatives.FirstOrDefault(a => a.SubjectId == subjectId);

            if (alternativeSubject == null)
            {
                // Subject not linked yet, add new
                var subjectName = _context.Subjects.FirstOrDefault(s => s.Id == subjectId)?.Name ?? "Unknown";
                var subjectLevel = model.AlternativeSubjectLevels.ContainsKey(subjectId) ? model.AlternativeSubjectLevels[subjectId] : defaultLevel; // Use a default level or handle accordingly

                _context.AlternativeSubjects.Add(new AlternativeSubject
                {
                    CourseId = courseId,
                    SubjectId = subjectId,
                    AlternativeSubjectLevel = subjectLevel,
                    AlternativeSubjectName = subjectName
                });
            }
            else
            {
                // Update existing subject
                if (model.AlternativeSubjectLevels.ContainsKey(subjectId))
                {
                    alternativeSubject.AlternativeSubjectLevel = model.AlternativeSubjectLevels[subjectId];
                }
            }
        }

        // Remove unselected subjects
        _context.AlternativeSubjects.RemoveRange(existingAlternatives.Where(a => !model.SelectedAlternativeSubjects.Contains(a.SubjectId)));

        await _context.SaveChangesAsync();
    }



    // GET: /Admin/DeleteCourse/5
    public async Task<IActionResult> DeleteCourse(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .Include(c => c.University)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    [HttpPost, ActionName("DeleteCourse")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Course '{course.CourseName}' deleted successfully.");
        }

        return RedirectToAction(nameof(Index));
    }



}
