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
    public class AddTopicControllerTests
    {
        [Fact]
        public async Task Index_ReturnsRedirectToHomeIndex_WhenModelStateIsValid()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.
            var controller = new AddTopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            var viewModel = new AddTopicViewModel // Создание модели представления для тестирования.
            {
                TopicCode = 1,
                NameTopic = "New Topic"
            };

            // Act
            var result = await controller.Index(viewModel); // Вызов метода Index контроллера.

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); // Проверка, что результат является перенаправлением.
            Assert.Equal("Index", redirectToActionResult.ActionName); // Проверка, что перенаправление происходит на действие Index.
            Assert.Equal("Home", redirectToActionResult.ControllerName); // Проверка, что перенаправление происходит на контроллер Home.
        }

        [Fact]
        public async Task Index_ReturnsViewWithViewModel_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.
            var controller = new AddTopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            var viewModel = new AddTopicViewModel(); // Создание пустой модели представления для тестирования.
            controller.ModelState.AddModelError("NameTopic", "Required"); // Добавление ошибки в ModelState.

            // Act
            var result = await controller.Index(viewModel); // Вызов метода Index контроллера.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является представлением.
            Assert.Equal(viewModel, viewResult.Model); // Проверка, что модель представления возвращается обратно в представление.
        }
    }
}
