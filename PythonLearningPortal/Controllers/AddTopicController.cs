using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class AddTopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public AddTopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: AddTopic/Index
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddTopicViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Преобразование модели представления в модель данных
                    var topic = new Темы
                    {
                        Код_темы = model.TopicCode,
                        Название_темы = model.NameTopic
                    };

                    // Добавление модели в контекст данных и сохранение изменений в базу данных
                    _context.Темы.Add(topic);
                    await _context.SaveChangesAsync();

                    // Перенаправление на другую страницу после успешного добавления
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateException ex)
                {
                    // Обработка ошибок, если что-то пошло не так при сохранении в базу данных
                    // Здесь можно добавить дополнительную обработку, если необходимо
                    ModelState.AddModelError("", "Произошла ошибка при добавлении новой темы. Пожалуйста, попробуйте еще раз.");
                    return View(model);
                }
            }
            // Если ModelState не валиден, возвращаем представление с моделью, чтобы пользователь мог исправить ошибки
            return View(model);
        }
    }
}
