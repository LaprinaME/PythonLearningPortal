using Xunit; // Подключение библиотеки xUnit для написания и выполнения тестов.
using Microsoft.AspNetCore.Mvc; // Подключение библиотеки для работы с контроллерами и действиями в ASP.NET Core.
using Moq; // Подключение библиотеки Moq для создания mock-объектов.
using System.Threading.Tasks; // Подключение пространства имен для работы с задачами.
using Microsoft.EntityFrameworkCore; // Подключение библиотеки для работы с Entity Framework Core.
using PythonLearningPortal.Controllers; // Подключение пространства имен, в котором находится AccountController.
using PythonLearningPortal.Models; // Подключение пространства имен с моделями данных.
using PythonLearningPortal.DataContext; // Подключение пространства имен с контекстом базы данных.
using PythonLearningPortal.ViewModels; // Подключение пространства имен с моделями представления.

namespace PythonLearningPortal.Test // Определение нового пространства имен для тестов.
{
    public class AccountControllerTests // Объявление класса для тестов контроллера AccountController.
    {
        [Fact] // Атрибут, указывающий, что метод является тестовым и должен выполняться как тест xUnit.
        public async Task Login_InvalidLogin_ReturnsViewWithErrorMessage() // Объявление тестового метода для проверки сценария с неверным логином.
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.
            var mockDbSet = new Mock<DbSet<Аккаунты>>(); // Создание mock-объекта DbSet для аккаунтов.

            // Настройка mock-объекта контекста для возврата mock-объекта DbSet при обращении к свойству Аккаунты.
            mockContext.Setup(c => c.Аккаунты).Returns(mockDbSet.Object);

            // Создание экземпляра AccountController с использованием mock-контекста.
            var controller = new AccountController(mockContext.Object);

            // Создание модели LoginViewModel с некорректными данными.
            var model = new LoginViewModel
            {
                Login = "wrongUser",
                Password = "wrongPassword"
            };

            // Act
            var result = await controller.Login(model); // Вызов метода Login контроллера.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является объектом типа ViewResult.
            Assert.Equal(model, viewResult.Model); // Проверка, что модель представления совпадает с исходной моделью.
            Assert.True(controller.ModelState.ErrorCount > 0); // Проверка, что в ModelState есть ошибки.
        }
    }
}
