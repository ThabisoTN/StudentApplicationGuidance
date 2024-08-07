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
    public class SelectSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserSubjectService _userSubjectService;

        public SelectSubjectsController(ApplicationDbContext context, UserSubjectService userSubjectService)
        {
            _context = context;
            _userSubjectService = userSubjectService;
        }

        // GET: /SelectSubjects/Create
        public async Task<IActionResult> Create()
        {
            var subjects = await _userSubjectService.GetAllSubjects();
            ViewBag.Subjects = subjects;

            return View();
        }

        // POST: /SelectSubjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SelectSubjectsView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        return Unauthorized();
                    }

                    // Get existing subjects for the user
                    var existingUserSubjects = await _context.UserSubjects
                        .Where(us => us.UserId == userId)
                        .Select(us => us.SubjectId)
                        .ToListAsync();

                    var subjectIds = new HashSet<int>(); // Use HashSet to avoid duplicates
                    var levels = new List<int>();
                    var validationMessage = string.Empty;

                    for (int i = 1; i <= 7; i++)
                    {
                        var subjectIdProp = typeof(SelectSubjectsView).GetProperty($"Subject{i}");
                        var levelProp = typeof(SelectSubjectsView).GetProperty($"Subject{i}Level");

                        var subjectId = (string)subjectIdProp.GetValue(model);
                        var level = (int)levelProp.GetValue(model);

                        if (!string.IsNullOrEmpty(subjectId) && level > 0)
                        {
                            int id = int.Parse(subjectId); // Ensure proper conversion

                            // Check for duplicates within the list being added
                            if (subjectIds.Contains(id))
                            {
                                TempData["ErrorMessage"] = $"Duplicate subject with ID {id} detected.";
                           
                                return RedirectToAction("Create");
                            }

                            // Check if the subject is already saved for the user
                            if (existingUserSubjects.Contains(id))
                            {
                                TempData["ErrorMessage"] = $"Subject with ID {id} already exists.";
                                validationMessage = "Subject already exist";
                                return RedirectToAction("Create");
                            }

                            subjectIds.Add(id);
                            levels.Add(level);
                        }
                    }

                    // Validate the required subjects
                    bool hasEnglish = subjectIds.Contains(1) || subjectIds.Contains(2);
                    bool hasMath = subjectIds.Contains(3) || subjectIds.Contains(4);
                    bool hasZuluOrAfrikaans = subjectIds.Contains(5) || subjectIds.Contains(6) || subjectIds.Contains(7) || subjectIds.Contains(8);
                    bool hasLifeOrientation = subjectIds.Contains(9);

                    if (!hasEnglish)
                    {
                        validationMessage += "You must add either English Home Language or English First Additional Language.\\n";
                    }
                    if (!hasMath)
                    {
                        validationMessage += "You must add either Mathematics or Mathematics Literacy.\\n";
                    }
                    if (!hasZuluOrAfrikaans)
                    {
                        validationMessage += "You must add either IsiZulu Home Language, IsiZulu First Additional Language, Afrikaans Home Language, or Afrikaans First Additional Language.\\n";
                    }
                    if (!hasLifeOrientation)
                    {
                        validationMessage += "Life orientation is compulsory!\\n";
                    }

                    if (!string.IsNullOrEmpty(validationMessage))
                    {
                        TempData["ValidationMessage"] = validationMessage;
                    }
                    else
                    {
                        // Add user subjects
                        await _userSubjectService.AddUserSubjectsAsync(userId, subjectIds.ToList(), levels);
                        return RedirectToAction("ViewSubjects", "UserSubjects");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving subjects: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Subjects = await _userSubjectService.GetAllSubjects();
            return View(model);
        }




        // Retrieving user subjects and levels
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userSubjects = await _context.UserSubjects.Include(us => us.Subject).Where(us => us.UserId == userId).ToListAsync();

            return View(userSubjects);
        }
    }
}
