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

        public IActionResult Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";

            var exercises = from e in _context.Упражнения
                            select e;

            if (!string.IsNullOrEmpty(searchString))
            {
                exercises = exercises.Where(e => e.Название_упражнения.Contains(searchString) || e.Описание.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    exercises = exercises.OrderByDescending(e => e.Название_упражнения);
                    break;
                case "Id":
                    exercises = exercises.OrderBy(e => e.Код_упражнения);
                    break;
                case "id_desc":
                    exercises = exercises.OrderByDescending(e => e.Код_упражнения);
                    break;
                default:
                    exercises = exercises.OrderBy(e => e.Название_упражнения);
                    break;
            }

            return View(exercises.ToList());
        }
    }
}
