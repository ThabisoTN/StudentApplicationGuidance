using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
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
            return View("Error"); // Make sure you have an Error view
        }
    }

}
