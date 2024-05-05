using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.ViewModels;

namespace PythonLearningPortal.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: /Registration/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Registration/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            // Logic for user registration
            return RedirectToAction("Index", "Home");
        }
    }
}
