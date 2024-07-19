using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApplicationGuidance.Data;
using StudentApplicationGuidance.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentApplicationGuidance.Controllers
{
    public class UserSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserSubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserSubjects/ViewSubjects
        public async Task<IActionResult> ViewSubjects()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch user subjects with related subject information
            var userSubjects = await _context.UserSubjects.Include(us => us.Subject).Where(us => us.UserId == userId).ToListAsync();

            return View(userSubjects);
        }
    }
}
