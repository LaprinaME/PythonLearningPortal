using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System;
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

        // GET: EditTopic/Index/5
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Находим тему по идентификатору
            var topic = await _context.Темы.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            // Преобразование модели данных в модель представления
            var viewModel = new EditTopicViewModel
            {
                TopicCode = topic.Код_темы,
                NameTopic = topic.Название_темы
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, EditTopicViewModel viewModel)
        {
            if (id != viewModel.TopicCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Находим тему по идентификатору
                    var topic = await _context.Темы.FindAsync(id);
                    if (topic == null)
                    {
                        return NotFound();
                    }

                    // Обновляем данные темы из модели представления
                    topic.Название_темы = viewModel.NameTopic;

                    // Сохраняем изменения в базе данных
                    _context.Update(topic);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
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
            return View(viewModel);
        }

        private bool TopicExists(int id)
        {
            return _context.Темы.Any(e => e.Код_темы == id);
        }
    }
}
