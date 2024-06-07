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
    public class DeleteUsersControllerTests
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
        public async Task Index_ReturnsViewResult_WithListOfUsers()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Пользователи.Add(new Пользователи { Код_пользователя = 1, ФИО = "User1" });
                context.Пользователи.Add(new Пользователи { Код_пользователя = 2, ФИО = "User2" });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteUsersController(context);

                // Act
                var result = await controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<DeleteUsersViewModel>>(viewResult.Model);
                Assert.Equal(2, model.Count);
                Assert.Equal("User1", model[0].ФИО);
                Assert.Equal("User2", model[1].ФИО);
            }
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteUsersController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_RedirectsToIndex_WhenUserIsDeleted()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Пользователи.Add(new Пользователи { Код_пользователя = 1, ФИО = "User1" });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteUsersController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);

                var user = await context.Пользователи.FindAsync(1);
                Assert.Null(user);
            }
        }
    }
}
