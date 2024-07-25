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

                    // Check if the user already has subjects
                    var existingSubjects = await _context.UserSubjects.AnyAsync(us => us.UserId == userId);

                    if (existingSubjects)
                    {
                        // User already has subjects, redirect to view subjects action
                        return RedirectToAction("ViewSubjects", "UserSubjects");
                    }

                    var subjectIds = new List<int>();
                    var levels = new List<int>();

                    for (int i = 1; i <= 7; i++)
                    {
                        var subjectIdProp = typeof(SelectSubjectsView).GetProperty($"Subject{i}");
                        var levelProp = typeof(SelectSubjectsView).GetProperty($"Subject{i}Level");

                        var subjectId = (string)subjectIdProp.GetValue(model);
                        var level = (int)levelProp.GetValue(model);

                        if (!string.IsNullOrEmpty(subjectId) && level > 0)
                        {
                            subjectIds.Add(int.Parse(subjectId)); // Ensure proper conversion
                            levels.Add(level);
                        }
                    }

                    // Validate the required subjects
                    bool hasEnglish = subjectIds.Contains(1) || subjectIds.Contains(2);
                    bool hasMath = subjectIds.Contains(3) || subjectIds.Contains(4);
                    bool hasZuluOrAfrikaans = subjectIds.Contains(5) || subjectIds.Contains(6) || subjectIds.Contains(7) || subjectIds.Contains(8);
                    bool hasLifeOrientation = subjectIds.Contains(9);

                    string validationMessage = string.Empty;

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
                        validationMessage += "life orientation is compolsary!!!";
                    }


                    if (!string.IsNullOrEmpty(validationMessage))
                    {
                        ViewBag.ValidationMessage = validationMessage;
                        ModelState.AddModelError("", "Validation errors occurred.");
                    }
                    else
                    {
                        await _userSubjectService.AddUserSubjectsAsync(userId, subjectIds, levels);
                        return RedirectToAction("ViewSubjects", "UserSubjects");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving subjects: {ex.Message}");
                }
            }

            // If we get here, something went wrong; redisplay the form with current model state
            var subjects = await _userSubjectService.GetAllSubjects();
            ViewBag.Subjects = subjects;
            return View(model);
        }







        //Retrieving user subject and level
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userSubjects = await _context.UserSubjects.Include(us => us.Subject).Where(us => us.UserId == userId).ToListAsync();

            return View(userSubjects);
        }
    }
}
