using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Threading.Tasks;

namespace PythonLearningPortal.Test
{
    public class DeleteSubtopicControllerTests
    {
        [Fact]
        public async Task Delete_ReturnsRedirectToIndex_WhenSubtopicExists()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var subtopicId = 1;
            var existingSubtopic = new Подтемы { Код_подтемы = subtopicId };
            mockContext.Setup(c => c.Подтемы.FindAsync(subtopicId)).ReturnsAsync(existingSubtopic); // Настройка mock-объекта контекста для возврата существующей подтемы.

            var controller = new DeleteSubtopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(subtopicId); // Вызов метода Delete контроллера.

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); // Проверка, что результат является перенаправлением.
            Assert.Equal("Index", redirectToActionResult.ActionName); // Проверка, что перенаправление происходит на действие Index.
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenSubtopicDoesNotExist()
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.

            var subtopicId = 1;
            mockContext.Setup(c => c.Подтемы.FindAsync(subtopicId)).ReturnsAsync((Подтемы)null); // Настройка mock-объекта контекста для возврата null при поиске подтемы.

            var controller = new DeleteSubtopicController(mockContext.Object); // Создание экземпляра контроллера с использованием mock-контекста.

            // Act
            var result = await controller.Delete(subtopicId); // Вызов метода Delete контроллера.

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result); // Проверка, что результат является NotFoundResult.
            Assert.Equal(404, notFoundResult.StatusCode); // Проверка, что код статуса соответствует "Not Found".
        }
    }
}
