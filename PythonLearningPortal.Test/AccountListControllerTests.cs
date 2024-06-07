using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Tests
{
    public class AccountListControllerTests
    {
        private DbContextOptions<PythonLearningPortalContext> CreateNewContextOptions()
        {
            // Create a new service provider, and therefore a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<PythonLearningPortalContext>();
            builder.UseInMemoryDatabase("TestDatabase")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithAListOfAccounts()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Аккаунты.AddRange(
                    new Аккаунты { Логин = "testuser1", Пароль = "password1", Код_роли = 1 },
                    new Аккаунты { Логин = "testuser2", Пароль = "password2", Код_роли = 1 },
                    new Аккаунты { Логин = "exampleuser", Пароль = "password3", Код_роли = 2 }
                );
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AccountListController(context);

                // Act
                var result = await controller.Index(null, null);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Аккаунты>>(viewResult.ViewData.Model);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Index_WithFilter_ReturnsFilteredAccounts()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Аккаунты.AddRange(
                    new Аккаунты { Логин = "testuser1", Пароль = "password1", Код_роли = 1 },
                    new Аккаунты { Логин = "testuser2", Пароль = "password2", Код_роли = 1 },
                    new Аккаунты { Логин = "exampleuser", Пароль = "password3", Код_роли = 2 }
                );
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AccountListController(context);

                // Act
                var result = await controller.Index("test", null);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Аккаунты>>(viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());
                Assert.All(model, a => Assert.Contains("test", a.Логин));
            }
        }

        [Fact]
        public async Task Index_WithLengthFilter_ReturnsFilteredAccounts()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Аккаунты.AddRange(
                    new Аккаунты { Логин = "short", Пароль = "password1", Код_роли = 1 },
                    new Аккаунты { Логин = "verylongusername", Пароль = "password2", Код_роли = 1 },
                    new Аккаунты { Логин = "mediumlen", Пароль = "password3", Код_роли = 2 }
                );
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AccountListController(context);

                // Act
                var result = await controller.Index(null, 8);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Аккаунты>>(viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());
                Assert.All(model, a => Assert.True(a.Логин.Length <= 8));
            }
        }
    }
}
