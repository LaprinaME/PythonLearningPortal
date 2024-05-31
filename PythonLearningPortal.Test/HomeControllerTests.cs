using Xunit; // Подключение библиотеки xUnit для написания и выполнения тестов.
using Microsoft.AspNetCore.Mvc; // Подключение библиотеки для работы с контроллерами и действиями в ASP.NET Core.
using PythonLearningPortal.Controllers; // Подключение пространства имен, в котором находится HomeController.

namespace PythonLearningPortal.Test // Определение нового пространства имен для тестов.
{
    public class HomeControllerTests // Объявление класса для тестов контроллера HomeController.
    {
        [Fact] // Атрибут, указывающий, что метод является тестовым и должен выполняться как тест xUnit.
        public void Index_ReturnsViewResult() // Объявление тестового метода, проверяющего метод Index контроллера.
        {
            // Arrange
            var controller = new HomeController(); // Создание экземпляра HomeController для тестирования.

            // Act
            var result = controller.Index(); // Вызов метода Index контроллера и сохранение результата.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является объектом типа ViewResult.
            Assert.NotNull(viewResult); // Проверка, что результат не равен null.
        }
    }
}
