using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.ModelView;
using StudentApplicationGuidance.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentApplicationGuidance.Controllers
{
    [Authorize] // Ensure user is authenticated
    public class SubjectController : Controller
    {
        private readonly UserSubjectService _userSubjectService;
        private readonly SubjectService _subjectService;

        public SubjectController(UserSubjectService userSubjectService, SubjectService subjectService)
        {
            _userSubjectService = userSubjectService;
            _subjectService = subjectService;
        }

        // GET: /Subject/Selection
        public async Task<IActionResult> Selection()
        {
            var availableSubjects = await _subjectService.GetAllSubjectsAsync();
            var model = new UserSubjectSelectionModel { AvailableSubjects = availableSubjects };
            return View(model);
        }

        // POST: /Subject/SaveSubjects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSubjects(UserSubjectSelectionModel model)
        {
            if (ModelState.IsValid)
            {
                // Get current user's ID
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Save selected subjects for the user
                var selectedSubjects = new List<int>();
                foreach (var selectedSubject in model.SelectedSubjects)
                {
                    selectedSubjects.Add(selectedSubject.SubjectId);
                }

                await _userSubjectService.AddUserSubjectsAsync(userId, selectedSubjects, model.SelectedSubjects[0].Level);

                return RedirectToAction("Index", "Home"); // Redirect to home page or another appropriate action
            }

            // If model state is not valid, redisplay the form with errors
            var availableSubjects = await _subjectService.GetAllSubjectsAsync();
            model.AvailableSubjects = availableSubjects;
            return View("Selection", model);
        }

        // GET: /Subject/UserSubjects
        public async Task<IActionResult> UserSubjects()
        {
            // Get current user's ID
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Retrieve and display user's selected subjects
            var userSubjects = await _userSubjectService.GetUserSubjectsAsync(userId);
            var userSubjectViews = new List<UserSubjectView>();

            foreach (var userSubject in userSubjects)
            {
                userSubjectViews.Add(new UserSubjectView
                {
                    UserSubjectId = userSubject.Id,
                    SubjectId = userSubject.Subject.Id,
                    SubjectName = userSubject.Subject.Name,
                    UserId = userSubject.User.Id,
                    UserName = userSubject.User.UserName,
                    Level = userSubject.Level
                });
            }

            return View(userSubjectViews);
        }
    }
}
