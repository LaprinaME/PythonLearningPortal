using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;

namespace PythonLearningPortal.Controllers
{
    public class TestsController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public TestsController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Получаем список всех тестов из базы данных
            List<Тесты> tests = _context.Тесты.ToList();

            return View(tests); // Передаем список тестов в представление
        }

        // Другие методы контроллера (например, для отправки ответов на тесты и других действий) можно добавить здесь
    }
}
