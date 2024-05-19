using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Route("EditSubtopic")]
        public async Task<IActionResult> Index()
        {
            // Получение списка тем из базы данных
            var topics = await _context.Темы.Select(t => new TopicViewModel
            {
                TopicId = t.Код_темы,
                TopicTitle = t.Название_темы
            }).ToListAsync();

            // Получение списка подтем из базы данных
            var subtopics = await _context.Подтемы.Select(s => new SubtopicViewModel
            {
                SubtopicId = s.Код_подтемы,
                SubtopicTitle = s.Название_подтемы,
                TopicId = s.Код_темы
            }).ToListAsync();

            // Создание модели представления
            var viewModel = new EditSubtopicViewModel
            {
                Topics = topics, // Заполнение списка тем в модели представления
                Subtopics = subtopics // Заполнение списка подтем в модели представления
            };

            return View(viewModel);
        }

        // GET: EditSubtopic/Index
        [HttpGet]
        public async Task<IActionResult> Index(int? subtopicId)
        {
            if (subtopicId == null)
            {
                Console.WriteLine("Идентификатор подтемы равен нулю");
                return NotFound();
            }

            var subtopic = await _context.Подтемы.FindAsync(subtopicId);

            if (subtopic == null)
            {
                Console.WriteLine($"Подтема с ID {subtopicId} не найдена");
                return NotFound();
            }

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
                var subtopic = await _context.Подтемы.FindAsync(viewModel.SubtopicCode);

                if (subtopic == null)
                {
                    return NotFound();
                }

                subtopic.Название_подтемы = viewModel.NameSubtopic;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            // При ошибке валидации возвращаем представление с переданной моделью
            return View(viewModel);
        }
    }
}
