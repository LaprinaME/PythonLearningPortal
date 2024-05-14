using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class EditSubtopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public EditSubtopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View("Index");
        }

        [Route("EditSubtopic")]

        // GET: EditSubtopic/Index
        [HttpGet]
        public async Task<IActionResult> Index(int? subtopicId)
        {
            if (subtopicId == null)
            {
                return NotFound();
            }

            // Находим подтему по коду
            var subtopic = await _context.Подтемы.FindAsync(subtopicId);

            if (subtopic == null)
            {
                return NotFound();
            }

            // Создаем модель представления для редактирования подтемы
            var model = new EditSubtopicViewModel
            {
                SubtopicCode = subtopic.Код_подтемы,
                TopicCode = subtopic.Код_темы,
                NameSubtopic = subtopic.Название_подтемы
            };

            return View(model);
        }

        // POST: EditSubtopic/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EditSubtopicViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Находим подтему по коду
                var subtopic = await _context.Подтемы.FindAsync(viewModel.SubtopicCode);

                if (subtopic == null)
                {
                    return NotFound();
                }

                // Обновляем данные подтемы
                subtopic.Название_подтемы = viewModel.NameSubtopic;

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }
    }
}
