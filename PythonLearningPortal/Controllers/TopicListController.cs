using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class TopicListController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public TopicListController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: TopicList
        public async Task<IActionResult> Index()
        {
            var topics = await _context.Темы.ToListAsync();
            return View(topics);
        }
    }
}
