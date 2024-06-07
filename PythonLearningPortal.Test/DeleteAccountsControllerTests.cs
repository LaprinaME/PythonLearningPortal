using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Tests
{
    public class DeleteAccountsControllerTests
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
        public async Task Index_ReturnsViewResult_WithListOfAccounts()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Аккаунты.Add(new Аккаунты { Код_аккаунта = 1, Логин = "user1", Пароль = "pass1", Код_роли = 1 });
                context.Аккаунты.Add(new Аккаунты { Код_аккаунта = 2, Логин = "user2", Пароль = "pass2", Код_роли = 2 });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteAccountsController(context);

                // Act
                var result = await controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<AccountViewModel>>(viewResult.Model);
                Assert.Equal(2, model.Count);
                Assert.Equal("user1", model[0].Логин);
                Assert.Equal("user2", model[1].Логин);
            }
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteAccountsController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_RedirectsToIndex_WhenAccountIsDeleted()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Аккаунты.Add(new Аккаунты { Код_аккаунта = 1, Логин = "user1", Пароль = "pass1", Код_роли = 1 });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteAccountsController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);

                var account = await context.Аккаунты.FindAsync(1);
                Assert.Null(account);
            }
        }
    }
}
