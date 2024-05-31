using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Threading.Tasks;

namespace PythonLearningPortal.Test
{
    public class DeleteAccountsControllerTests
    {
        [Fact]
        public async Task Delete_ReturnsRedirectToIndex_WhenAccountExists()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var accountId = 1;
            var existingAccount = new Аккаунты { Код_аккаунта = accountId };
            mockContext.Setup(c => c.Аккаунты.FindAsync(accountId)).ReturnsAsync(existingAccount); // Настройка mock-объекта контекста для возврата существующей учетной записи.

            var controller = new DeleteAccountsController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(accountId); // Вызов метода Delete контроллера.

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); // Проверка, что результат является перенаправлением.
            Assert.Equal("Index", redirectToActionResult.ActionName); // Проверка, что перенаправление происходит на действие Index.
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var accountId = 1;
            mockContext.Setup(c => c.Аккаунты.FindAsync(accountId)).ReturnsAsync((Аккаунты)null); // Настройка mock-объекта контекста для возврата null при поиске учетной записи.

            var controller = new DeleteAccountsController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(accountId); // Вызов метода Delete контроллера.

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result); // Проверка, что результат является NotFoundResult.
            Assert.Equal(404, notFoundResult.StatusCode); // Проверка, что код статуса соответствует "Not Found".
        }
    }
}

