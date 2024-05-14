using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class AddSubtopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public AddSubtopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }
        [Route("AddSubtopic")]
        public async Task<IActionResult> Index()
        {
            return View("Index");
        }

        // GET: AddSubtopic/Index
        [HttpGet]
        public async Task<IActionResult> Index(int? topicId)
        {
            if (topicId == null)
            {
                return NotFound();
            }

            var topic = await _context.Темы.FirstOrDefaultAsync(t => t.Код_темы == topicId);

            if (topic == null)
            {
                return NotFound();
            }

            var viewModel = new AddSubtopicViewModel
            {
                TopicId = topicId.Value,
                SubtopicId = 0, // Предполагая, что SubtopicId является новым и будет сгенерирован базой данных
                SubtopicName = "" // Инициализировать пустой строкой
            };

            return View(viewModel);
        }

        // POST: AddSubtopic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddSubtopicViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var subtopic = new Подтемы // Change Подтемы to Subtopic
                {
                    Название_подтемы = viewModel.SubtopicName,
                    Код_темы = viewModel.TopicId,
                    Код_подтемы = viewModel.SubtopicId
                };

                _context.Add(subtopic);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }
    }
}