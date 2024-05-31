using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class EditTopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public EditTopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // Метод для отображения списка тем
        public async Task<IActionResult> IndexAsync()
        {
            var viewModel = new EditTopicViewModel
            {
                Topics = await _context.Темы
                    .Select(t => new TopicViewModel
                    {
                        TopicId = t.Код_темы,
                        TopicTitle = t.Название_темы
                    }).ToListAsync(),
            };

            return View(viewModel);
        }

        // Метод для отображения формы редактирования темы
        [HttpPost]
        [Route("EditTopic/Details/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Темы.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            var viewModel = new EditTopicViewModel
            {
                TopicCode = topic.Код_темы,
                NameTopic = topic.Название_темы,
                Topics = await _context.Темы
                    .Select(t => new TopicViewModel
                    {
                        TopicId = t.Код_темы,
                        TopicTitle = t.Название_темы
                    }).ToListAsync(),
                Subtopics = await _context.Подтемы
                    .Where(st => st.Код_темы == id)
                    .Select(st => new SubtopicViewModel
                    {
                        SubtopicId = st.Код_подтемы,
                        SubtopicTitle = st.Название_подтемы
                    }).ToListAsync()
            };

            return View(viewModel);
        }

        // Метод для сохранения изменений в теме
        [HttpPost]
        [Route("EditTopic/Save/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id, EditTopicViewModel viewModel)
        {
            if (id != viewModel.TopicCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var topic = await _context.Темы.FindAsync(id);
                    if (topic == null)
                    {
                        return NotFound();
                    }

                    topic.Название_темы = viewModel.NameTopic;
                    _context.Update(topic);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("IndexAsync");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(viewModel.TopicCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            viewModel.Topics = await _context.Темы
                .Select(t => new TopicViewModel
                {
                    TopicId = t.Код_темы,
                    TopicTitle = t.Название_темы
                }).ToListAsync();

            viewModel.Subtopics = await _context.Подтемы
                .Where(st => st.Код_темы == id)
                .Select(st => new SubtopicViewModel
                {
                    SubtopicId = st.Код_подтемы,
                    SubtopicTitle = st.Название_подтемы
                }).ToListAsync();

            return View("Details", viewModel);
        }

        private bool TopicExists(int id)
        {
            return _context.Темы.Any(e => e.Код_темы == id);
        }
    }
}
