using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.Models;
using System.Collections.Generic;

namespace PythonLearningPortal.Controllers
{
    public class TestsListController : Controller
    {
        private readonly IList<Тесты> _tests;

        public TestsListController()
        {
            // Инициализация списка тестов для примера
            _tests = new List<Тесты>
            {
                new Тесты { Код_теста = 1, Название_теста = "Тест 1" },
                new Тесты { Код_теста = 2, Название_теста = "Тест 2" },
                new Тесты { Код_теста = 3, Название_теста = "Тест 3" }
            };
        }

        public IActionResult Index()
        {
            return View(_tests);
        }
    }
}
