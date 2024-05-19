using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";

            var tests = from t in _tests
                        select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                tests = tests.Where(t => t.Название_теста.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tests = tests.OrderByDescending(t => t.Название_теста);
                    break;
                case "Id":
                    tests = tests.OrderBy(t => t.Код_теста);
                    break;
                case "id_desc":
                    tests = tests.OrderByDescending(t => t.Код_теста);
                    break;
                default:
                    tests = tests.OrderBy(t => t.Название_теста);
                    break;
            }

            return View(tests.ToList());
        }
    }
}
