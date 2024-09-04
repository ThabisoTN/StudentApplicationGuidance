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
using Newtonsoft.Json;

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
            SAUniversities = await _context.SAUniversities.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UniversityName }).ToListAsync(),
            AllSubjects = await _context.Subjects.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToListAsync(),
            LevelOptions = Enumerable.Range(1, 7).Select(i => new LevelOption { Level = i, Description = $"Level {i}" }).ToList(),
            SelectedRequiredSubjects = course.SubjectRequired.Select(rs => rs.SubjectId).ToList(),
            SelectedAlternativeSubjects = course.AlternativeSubjects.Select(subj => subj.SubjectId).ToList(),
            RequiredSubjectLevels = course.SubjectRequired.ToDictionary(sr => sr.SubjectId, sr => sr.SubjectLevel),
            AlternativeSubjectLevels = course.AlternativeSubjects.ToDictionary(asub => asub.SubjectId, asub => asub.AlternativeSubjectLevel),
            NumberOfRequiredAlternativeSubjects = course.AlternativeSubjects.FirstOrDefault()?.NumberOfRequiredAlternativeSubjects ?? 0
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

        var course = await _context.Courses
            .Include(c => c.SubjectRequired)
            .Include(c => c.AlternativeSubjects)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is not valid.");
            LogModelStateErrors();
            await PopulateViewModel(model);
            return View(model);
        }

        try
        {
            var changes = new List<Change>();

            // Track changes in course properties
            if (course.CourseName != model.CourseName)
            {
                changes.Add(new Change { Field = "Course Name", OldValue = course.CourseName, NewValue = model.CourseName });
                course.CourseName = model.CourseName;
            }

            if (course.Description != model.Description)
            {
                changes.Add(new Change { Field = "Description", OldValue = course.Description, NewValue = model.Description });
                course.Description = model.Description;
            }

            if (course.Points != model.Points)
            {
                changes.Add(new Change { Field = "Points", OldValue = course.Points.ToString(), NewValue = model.Points.ToString() });
                course.Points = model.Points;
            }

            if (course.UniversityId != model.SelectedUniversityId)
            {
                var oldUniversity = await _context.SAUniversities.FindAsync(course.UniversityId);
                var newUniversity = await _context.SAUniversities.FindAsync(model.SelectedUniversityId);
                changes.Add(new Change { Field = "University", OldValue = oldUniversity?.UniversityName, NewValue = newUniversity?.UniversityName });
                course.UniversityId = model.SelectedUniversityId;
            }

            if (course.NumberOfRequiredAlternativeSubjects != model.NumberOfRequiredAlternativeSubjects)
            {
                changes.Add(new Change { Field = "Number of Required Alternative Subjects", OldValue = course.NumberOfRequiredAlternativeSubjects.ToString(), NewValue = model.NumberOfRequiredAlternativeSubjects.ToString() });
                course.NumberOfRequiredAlternativeSubjects = model.NumberOfRequiredAlternativeSubjects;
            }

            // Track changes in required subjects
            var originalRequiredSubjects = course.SubjectRequired.Select(sr => sr.SubjectId).ToList();
            var addedRequiredSubjects = model.SelectedRequiredSubjects.Except(originalRequiredSubjects).ToList();
            var removedRequiredSubjects = originalRequiredSubjects.Except(model.SelectedRequiredSubjects).ToList();

            if (addedRequiredSubjects.Any() || removedRequiredSubjects.Any())
            {
                var addedSubjectsNames = await _context.Subjects
                    .Where(s => addedRequiredSubjects.Contains(s.Id))
                    .Select(s => s.Name)
                    .ToListAsync();

                var removedSubjectsNames = await _context.Subjects
                    .Where(s => removedRequiredSubjects.Contains(s.Id))
                    .Select(s => s.Name)
                    .ToListAsync();

                if (addedSubjectsNames.Any())
                {
                    changes.Add(new Change
                    {
                        Field = "Required Subjects Added",
                        OldValue = string.Empty,
                        NewValue = string.Join(", ", addedSubjectsNames)
                    });
                }

                if (removedSubjectsNames.Any())
                {
                    changes.Add(new Change
                    {
                        Field = "Required Subjects Removed",
                        OldValue = string.Join(", ", removedSubjectsNames),
                        NewValue = string.Empty
                    });
                }
            }

            // Track changes in alternative subjects
            var originalAlternativeSubjects = course.AlternativeSubjects.Select(asub => asub.SubjectId).ToList();
            var addedAlternativeSubjects = model.SelectedAlternativeSubjects.Except(originalAlternativeSubjects).ToList();
            var removedAlternativeSubjects = originalAlternativeSubjects.Except(model.SelectedAlternativeSubjects).ToList();

            if (addedAlternativeSubjects.Any() || removedAlternativeSubjects.Any())
            {
                var addedSubjectsNames = await _context.Subjects
                    .Where(s => addedAlternativeSubjects.Contains(s.Id))
                    .Select(s => s.Name)
                    .ToListAsync();

                var removedSubjectsNames = await _context.Subjects
                    .Where(s => removedAlternativeSubjects.Contains(s.Id))
                    .Select(s => s.Name)
                    .ToListAsync();

                if (addedSubjectsNames.Any())
                {
                    changes.Add(new Change
                    {
                        Field = "Alternative Subjects Added",
                        OldValue = string.Empty,
                        NewValue = string.Join(", ", addedSubjectsNames)
                    });
                }

                if (removedSubjectsNames.Any())
                {
                    changes.Add(new Change
                    {
                        Field = "Alternative Subjects Removed",
                        OldValue = string.Join(", ", removedSubjectsNames),
                        NewValue = string.Empty
                    });
                }
            }

            // Update required subjects and alternative subjects
            await UpdateRequiredSubjects(model, course.CourseId);
            await UpdateAlternativeSubjects(model, course.CourseId);

            _context.Update(course);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Course '{course.CourseName}' updated successfully.");

            TempData["SuccessMessage"] = "Update Successful!";
            TempData["Changes"] = JsonConvert.SerializeObject(changes); // Serialize the list to JSON

            return RedirectToAction(nameof(EditSuccess));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the course.");
            ModelState.AddModelError("", "An error occurred while updating the course. Please try again.");
            await PopulateViewModel(model);
            return View(model);
        }
    }


    private async Task UpdateRequiredSubjects(CourseViewModel model, int courseId)
    {
        var existingSubjects = _context.SubjectRequireds.Where(sr => sr.CourseId == courseId).ToList();
        var selectedSubjectIds = model.SelectedRequiredSubjects;

        // Add new required subjects that aren't already linked
        foreach (var subjectId in selectedSubjectIds)
        {
            if (!existingSubjects.Any(sr => sr.SubjectId == subjectId))
            {
                var subjectLevel = model.RequiredSubjectLevels.TryGetValue(subjectId, out int level) ? level : 1;
                _context.SubjectRequireds.Add(new SubjectRequired { CourseId = courseId, SubjectId = subjectId, SubjectLevel = subjectLevel });
            }
            else
            {
                // Update existing subject level
                var existingSubject = existingSubjects.FirstOrDefault(sr => sr.SubjectId == subjectId);
                if (existingSubject != null && model.RequiredSubjectLevels.TryGetValue(subjectId, out int level))
                {
                    existingSubject.SubjectLevel = level;
                }
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
                var subjectLevel = model.AlternativeSubjectLevels.TryGetValue(subjectId, out int level) ? level : 1;

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
                if (model.AlternativeSubjectLevels.TryGetValue(subjectId, out int level))
                {
                    alternativeSubject.AlternativeSubjectLevel = level;
                }
            }
        }

        // Remove unselected subjects
        _context.AlternativeSubjects.RemoveRange(existingAlternatives.Where(a => !model.SelectedAlternativeSubjects.Contains(a.SubjectId)));

        await _context.SaveChangesAsync();
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
