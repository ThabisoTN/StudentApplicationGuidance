using Microsoft.AspNetCore.Mvc;

namespace StudentApplicationGuidance.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
