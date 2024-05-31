using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Threading.Tasks;

namespace PythonLearningPortal.Test
{
    public class AddSubtopicControllerTests
    {
        [Fact]
        public async Task Create_ReturnsRedirectToHomeIndex_WhenModelStateIsValid()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.
            var controller = new AddSubtopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            var viewModel = new AddSubtopicViewModel // Создание модели представления для тестирования.
            {
                TopicId = 1,
                SubtopicId = 0,
                SubtopicName = "New Subtopic"
            };

            // Act
            var result = await controller.Create(viewModel); // Вызов метода Create контроллера.

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); // Проверка, что результат является перенаправлением.
            Assert.Equal("Index", redirectToActionResult.ActionName); // Проверка, что перенаправление происходит на действие Index.
            Assert.Equal("Home", redirectToActionResult.ControllerName); // Проверка, что перенаправление происходит на контроллер Home.
        }

        [Fact]
        public async Task Create_ReturnsViewWithViewModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.
            var controller = new AddSubtopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            var viewModel = new AddSubtopicViewModel(); // Создание пустой модели представления для тестирования.
            controller.ModelState.AddModelError("SubtopicName", "Required"); // Добавление ошибки в ModelState.

            // Act
            var result = await controller.Create(viewModel); // Вызов метода Create контроллера.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является представлением.
            Assert.Equal(viewModel, viewResult.Model); // Проверка, что модель представления возвращается обратно в представление.
        }
    }
}
