using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var subtopics = await _context.Подтемы.ToListAsync();
            var viewModel = new List<DeleteSubtopicViewModel>();

            foreach (var subtopic in subtopics)
            {
                viewModel.Add(new DeleteSubtopicViewModel
                {
                    SubtopicCode = subtopic.Код_подтемы,
                    NameSubtopic = subtopic.Название_подтемы,
                    TopicCode = subtopic.Код_темы
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int subtopicId)
        {
            var subtopic = await _context.Подтемы.FindAsync(subtopicId);

            if (subtopic == null)
            {
                return NotFound();
            }

            _context.Подтемы.Remove(subtopic);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
