using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Linq;

namespace PythonLearningPortal.Controllers
{
    public class ExercisesListController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public ExercisesListController(PythonLearningPortalContext context)
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
