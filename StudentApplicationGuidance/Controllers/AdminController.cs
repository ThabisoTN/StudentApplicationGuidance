using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using StudentApplicationGuidance.Services;

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

    //Get: admin/ViewUsers
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

    public async Task<IActionResult> University()
    {
        try
        {
            var university = await _context.universities.ToListAsync();
            return View(university);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving Universities: {ex.Message}");
            return View("Error");
        }
    }

    //Get:Admin/Courses
    public async Task<IActionResult> Courses()
    {
        try
        {
            var course = await _context.Courses.ToListAsync();
            return View(course);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving Course: {ex.Message}");
            return View("Error");
        }
    }


    //// GET: /Admin/AddCourse
    //public async Task<IActionResult> AddCourse()
    //{
    //    var subjects = await _context.Subjects.ToListAsync();
    //    var viewModel = new CourseViewModel
    //    {
    //        AvailableSubjects = subjects.Select(s => new SelectListItem
    //        {
    //            Value = s.SubjectId.ToString(),
    //            Text = s.SubjectName
    //        })
    //    };
    //    return View(viewModel);
    //}

    //// POST: /Admin/AddCourse
    //[HttpPost]
    //public async Task<IActionResult> AddCourse(CourseViewModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var course = new Course
    //        {
    //            CourseName = model.CourseName,
    //            UniversityId = model.UniversityId,
    //            RequiredSubjects = model.SelectedRequiredSubjects.Select(id => new RequiredSubject
    //            {
    //                SubjectId = id
    //            }).ToList(),
    //            AlternativeSubjects = model.SelectedAlternativeSubjects.Select(id => new AlternativeSubject
    //            {
    //                SubjectId = id
    //            }).ToList()
    //        };

    //        _context.Courses.Add(course);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction("Index");
    //    }

    //    // Repopulate subjects if the model is invalid
    //    model.AvailableSubjects = await _context.Subjects
    //        .Select(s => new SelectListItem
    //        {
    //            Value = s.SubjectId.ToString(),
    //            Text = s.SubjectName
    //        }).ToListAsync();
    //    return View(model);
    //}

}
