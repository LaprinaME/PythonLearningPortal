using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PythonLearningPortal.Test
{
    public class ExercisesListControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithListOfExercises()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var exercises = new List<Упражнения>
            {
                new Упражнения { Код_упражнения = 1, Название_упражнения = "Exercise 1", Описание = "Description 1" },
                new Упражнения { Код_упражнения = 2, Название_упражнения = "Exercise 2", Описание = "Description 2" }
            }.AsQueryable(); // Преобразование списка в запрос.

            var mockSet = new Mock<DbSet<Упражнения>>(); // Создание mock-сета для упражнений.
            mockSet.As<IQueryable<Упражнения>>().Setup(m => m.Provider).Returns(exercises.Provider); // Настройка провайдера.
            mockSet.As<IQueryable<Упражнения>>().Setup(m => m.Expression).Returns(exercises.Expression); // Настройка выражения.
            mockSet.As<IQueryable<Упражнения>>().Setup(m => m.ElementType).Returns(exercises.ElementType); // Настройка типа элемента.
            mockSet.As<IQueryable<Упражнения>>().Setup(m => m.GetEnumerator()).Returns(exercises.GetEnumerator()); // Настройка энумератора.

            mockContext.Setup(c => c.Упражнения).Returns(mockSet.Object); // Настройка mock-объекта контекста для возврата mock-сета упражнений.

            var controller = new ExercisesListController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = controller.Index(null, null); // Вызов метода Index контроллера.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является представлением.
            var model = Assert.IsAssignableFrom<IEnumerable<Упражнения>>(viewResult.ViewData.Model); // Проверка, что модель представления является списком упражнений.
            Assert.Equal(2, model.Count()); // Проверка, что список содержит два упражнения.
        }
    }
}

