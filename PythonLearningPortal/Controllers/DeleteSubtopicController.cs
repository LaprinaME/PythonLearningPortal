using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class DeleteSubtopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public DeleteSubtopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        [Route("DeleteSubtopic")]

        public async Task<IActionResult> Index()
        {
            return View("Index");
        }

        // GET: DeleteSubtopic
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                Console.WriteLine("Идентификатор подтемы равен нулю");
                return NotFound();
            }

            // Находим подтему по коду
            var subtopic = await _context.Подтемы.FindAsync(id);

            if (subtopic == null)
            {
                Console.WriteLine($"Подтема с идентификатором {id} не найдена");
                return NotFound();
            }

            // Создаем модель представления для удаления подтемы
            var model = new DeleteSubtopicViewModel
            {
                SubtopicCode = subtopic.Код_подтемы,
                NameSubtopic = subtopic.Название_подтемы
            };

            return View(model);
        }

        // POST: DeleteSubtopic
        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteSubtopicViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var subtopic = await _context.Подтемы.FindAsync(viewModel.SubtopicCode);

                if (subtopic == null)
                {
                    return NotFound();
                }

                _context.Подтемы.Remove(subtopic);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return BadRequest(ModelState);
        }
    }
}
