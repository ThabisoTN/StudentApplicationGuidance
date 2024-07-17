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
                    var existingSubjects = await _context.UserSubjects
                        .AnyAsync(us => us.UserId == userId);

                    if (existingSubjects)
                    {
                        // User already has subjects, redirect to view subjects action
                        return RedirectToAction("ViewSubjects", "UserSubjects");
                    }

                    var subjectIds = new List<int>();
                    var levels = new List<int>();

                    for (int i = 1; i <= 7; i++)
                    {
                        var subjectId = Convert.ToInt32(Request.Form[$"Subject{i}"]);
                        var level = Convert.ToInt32(Request.Form[$"Subject{i}Level"]);

                        subjectIds.Add(subjectId);
                        levels.Add(level);
                    }

                    await _userSubjectService.AddUserSubjectsAsync(userId, subjectIds, levels);
                    return RedirectToAction("ViewSubjects", "UserSubjects");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving subjects: {ex.Message}");
                }
            }

            var subjects = await _userSubjectService.GetAllSubjects();
            ViewBag.Subjects = subjects;
            return View(model);
        }





        //Retrieving user subject and level
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userSubjects = await _context.UserSubjects
                .Include(us => us.Subject)
                .Where(us => us.UserId == userId)
                .ToListAsync();

            return View(userSubjects);
        }
    }
}
