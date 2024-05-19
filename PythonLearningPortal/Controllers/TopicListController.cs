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
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            var topics = from t in _context.Темы
                         select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                topics = topics.Where(t => t.Название_темы.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    topics = topics.OrderByDescending(t => t.Название_темы);
                    break;
                case "Date":
                    topics = topics.OrderBy(t => t.Код_темы); // Replace with date property if available
                    break;
                case "date_desc":
                    topics = topics.OrderByDescending(t => t.Код_темы); // Replace with date property if available
                    break;
                default:
                    topics = topics.OrderBy(t => t.Название_темы);
                    break;
            }

            return View(await topics.AsNoTracking().ToListAsync());
        }
    }
}