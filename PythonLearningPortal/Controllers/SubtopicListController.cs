using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
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
        public async Task<IActionResult> Index()
        {
            var subtopics = await _context.Подтемы.ToListAsync();
            return View(subtopics);
        }
    }
}
