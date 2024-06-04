using Xunit; // Подключение библиотеки xUnit для написания и выполнения тестов.
using Microsoft.AspNetCore.Mvc; // Подключение библиотеки для работы с контроллерами и действиями в ASP.NET Core.
using Moq; // Подключение библиотеки Moq для создания mock-объектов.
using Microsoft.EntityFrameworkCore; // Подключение библиотеки для работы с Entity Framework Core.
using PythonLearningPortal.Controllers; // Подключение пространства имен, в котором находится AccountListController.
using PythonLearningPortal.Models; // Подключение пространства имен с моделями данных.
using PythonLearningPortal.DataContext; // Подключение пространства имен с контекстом базы данных.
using System.Collections.Generic; // Подключение библиотеки для работы с коллекциями.
using System.Linq; 
using System.Threading.Tasks; 

namespace PythonLearningPortal.Test // Определение нового пространства имен для тестов.
{
    public class AccountListControllerTests // Объявление класса для тестов контроллера AccountListController.
    {
        [Fact] // Атрибут, указывающий, что метод является тестовым и должен выполняться как тест xUnit.
        public async Task Index_ReturnsFilteredAccounts() // Объявление тестового метода для проверки фильтрации аккаунтов.
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.
            var mockDbSet = new Mock<DbSet<Аккаунты>>(); // Создание mock-объекта DbSet для аккаунтов.

            var data = new List<Аккаунты>
            {
                new Аккаунты { Логин = "user1", Пароль = "password1" },
                new Аккаунты { Логин = "user2", Пароль = "password2" },
                new Аккаунты { Логин = "testUser", Пароль = "password3" }
            }.AsQueryable();

            // Настройка mock-объекта DbSet для работы с данными.
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Настройка mock-объекта контекста для возврата mock-объекта DbSet при обращении к свойству Аккаунты.
            mockContext.Setup(c => c.Аккаунты).Returns(mockDbSet.Object);

            // Создание экземпляра AccountListController с использованием mock-контекста.
            var controller = new AccountListController(mockContext.Object);

            // Act
            var result = await controller.Index("user", null); // Вызов метода Index контроллера с фильтром "user".

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является объектом типа ViewResult.
            var model = Assert.IsAssignableFrom<List<Аккаунты>>(viewResult.Model); // Проверка, что модель представления является списком аккаунтов.
            Assert.Equal(2, model.Count); // Проверка, что количество отфильтрованных аккаунтов равно 2.
            Assert.Contains(model, a => a.Логин == "user1"); // Проверка, что отфильтрованный список содержит аккаунт с логином "user1".
            Assert.Contains(model, a => a.Логин == "user2"); // Проверка, что отфильтрованный список содержит аккаунт с логином "user2".
        }

        [Fact] // Атрибут, указывающий, что метод является тестовым и должен выполняться как тест xUnit.
        public async Task Index_ReturnsAccountsWithLimitedLength() // Объявление тестового метода для проверки ограничения длины логина.
        {
            // Arrange
            var mockContext = new Mock<PythonLearningPortalContext>(); // Создание mock-объекта контекста базы данных.
            var mockDbSet = new Mock<DbSet<Аккаунты>>(); // Создание mock-объекта DbSet для аккаунтов.

            var data = new List<Аккаунты>
            {
                new Аккаунты { Логин = "short", Пароль = "password1" },
                new Аккаунты { Логин = "mediumLength", Пароль = "password2" },
                new Аккаунты { Логин = "veryLongUsername", Пароль = "password3" }
            }.AsQueryable();

            // Настройка mock-объекта DbSet для работы с данными.
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Настройка mock-объекта контекста для возврата mock-объекта DbSet при обращении к свойству Аккаунты.
            mockContext.Setup(c => c.Аккаунты).Returns(mockDbSet.Object);

            // Создание экземпляра AccountListController с использованием mock-контекста.
            var controller = new AccountListController(mockContext.Object);

            // Act
            var result = await controller.Index(null, 10); // Вызов метода Index контроллера с ограничением длины логина 10 символов.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Проверка, что результат является объектом типа ViewResult.
            var model = Assert.IsAssignableFrom<List<Аккаунты>>(viewResult.Model); // Проверка, что модель представления является списком аккаунтов.
            Assert.Equal(2, model.Count); // Проверка, что количество аккаунтов с логином до 10 символов равно 2.
            Assert.Contains(model, a => a.Логин == "short"); // Проверка, что список содержит аккаунт с логином "short".
            Assert.Contains(model, a => a.Логин == "mediumLength"); // Проверка, что список содержит аккаунт с логином "mediumLength".
        }
    }
}

