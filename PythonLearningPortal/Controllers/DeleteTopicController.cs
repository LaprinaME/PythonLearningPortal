using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class DeleteTopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public DeleteTopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        [Route("DeleteTopic")]

        public async Task<IActionResult> Index()
        {
            return View("Index");
        }


        // GET: DeleteTopic/Index
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                // отладочное сообщение
                Console.WriteLine("Идентификатор темы равен нулю");
                return NotFound();
            }
            // Находим тему по коду
            var topic = await _context.Темы.FindAsync(id);

            if (topic == null)
            {
                // отладочное сообщение
                Console.WriteLine($"Тема с таким кодом {id} не найдена");
                return NotFound();
            }

            // Создаем модель представления для удаления темы
            var model = new DeleteTopicViewModel
            {
                TopicCode = topic.Код_темы,
                NameTopic = topic.Название_темы
            };

            return View(model);
        }

        // POST: DeleteTopic/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteTopicViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Find the topic by its code
                var topic = await _context.Темы.FindAsync(viewModel.TopicCode);

                if (topic == null)
                {
                    return NotFound();
                }

                // Remove the topic from the data context and save changes
                _context.Темы.Remove(topic);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return BadRequest(ModelState);
        }
    }
}