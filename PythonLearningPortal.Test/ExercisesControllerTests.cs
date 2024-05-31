using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;

namespace PythonLearningPortal.Test
{
    public class ExercisesControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithListOfExercises()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var exercises = new List<Упражнения>
            {
                new Упражнения { Код_упражнения = 1, Название_упражнения = "Exercise 1" },
                new Упражнения { Код_упражнения = 2, Название_упражнения = "Exercise 2" }
            };
            mockContext.Setup(c => c.Упражнения.ToList()).Returns(exercises); // Настройка mock-объекта контекста для возврата списка упражнений.

            var controller = new ExercisesController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = controller.Index(); // Вызов метода Index контроллера.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является представлением.
            var model = Assert.IsAssignableFrom<IEnumerable<Упражнения>>(viewResult.ViewData.Model); // Проверка, что модель представления является списком упражнений.
            Assert.Equal(2, model.Count()); // Проверка, что список содержит два упражнения.
        }
    }
}
