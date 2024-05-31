using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Threading.Tasks;

namespace PythonLearningPortal.Test
{
    public class DeleteUsersControllerTests
    {
        [Fact]
        public async Task Delete_ReturnsRedirectToIndex_WhenUserExists()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var userId = 1;
            var existingUser = new Пользователи { Код_пользователя = userId };
            mockContext.Setup(c => c.Пользователи.FindAsync(userId)).ReturnsAsync(existingUser); // Настройка mock-объекта контекста для возврата существующего пользователя.

            var controller = new DeleteUsersController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(userId); // Вызов метода Delete контроллера.

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); // Проверка, что результат является перенаправлением.
            Assert.Equal("Index", redirectToActionResult.ActionName); // Проверка, что перенаправление происходит на действие Index.
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var userId = 1;
            mockContext.Setup(c => c.Пользователи.FindAsync(userId)).ReturnsAsync((Пользователи)null); // Настройка mock-объекта контекста для возврата null при поиске пользователя.

            var controller = new DeleteUsersController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(userId); // Вызов метода Delete контроллера.

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result); // Проверка, что результат является NotFoundResult.
            Assert.Equal(404, notFoundResult.StatusCode); // Проверка, что код статуса соответствует "Not Found".
        }
    }
}
