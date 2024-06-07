using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Tests
{
    public class UsersListControllerTests
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
                var controller = new UsersListController(context);

                // Act
                var result = await controller.Index("User1");

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Пользователи>>(viewResult.ViewData.Model);
                Assert.Single(model);
                Assert.Equal("User1", model.First().ФИО);
            }
        }
    }
}
