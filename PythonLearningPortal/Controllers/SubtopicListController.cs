using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class SubtopicListController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public SubtopicListController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: SubtopicList
        public async Task<IActionResult> Index(string filter)
        {
            IQueryable<Подтемы> subtopicsQuery = _context.Подтемы;

            if (!string.IsNullOrEmpty(filter))
            {
                subtopicsQuery = subtopicsQuery.Where(s => s.Название_подтемы.Contains(filter));
            }

            var subtopics = await subtopicsQuery.ToListAsync();
            return View(subtopics);
        }
    }
}
