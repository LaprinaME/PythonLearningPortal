using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Linq;

namespace PythonLearningPortal.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public ExercisesController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var exercises = _context.Упражнения.ToList();
            return View(exercises);
        }
    }
}
