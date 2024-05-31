using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Threading.Tasks;

namespace PythonLearningPortal.Test
{
    public class DeleteTopicControllerTests
    {
        [Fact]
        public async Task Delete_ReturnsRedirectToIndex_WhenTopicExists()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var topicId = 1;
            var existingTopic = new Темы { Код_темы = topicId };
            mockContext.Setup(c => c.Темы.FindAsync(topicId)).ReturnsAsync(existingTopic); // Настройка mock-объекта контекста для возврата существующей темы.

            var controller = new DeleteTopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(topicId); // Вызов метода Delete контроллера.

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); // Проверка, что результат является перенаправлением.
            Assert.Equal("Index", redirectToActionResult.ActionName); // Проверка, что перенаправление происходит на действие Index.
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTopicDoesNotExist()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var topicId = 1;
            mockContext.Setup(c => c.Темы.FindAsync(topicId)).ReturnsAsync((Темы)null); // Настройка mock-объекта контекста для возврата null при поиске темы.

            var controller = new DeleteTopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(topicId); // Вызов метода Delete контроллера.

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result); // Проверка, что результат является NotFoundResult.
            Assert.Equal(404, notFoundResult.StatusCode); // Проверка, что код статуса соответствует "Not Found".
        }
    }
}
