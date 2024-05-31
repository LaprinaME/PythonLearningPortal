using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Test
{
    public class UsersListControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new PythonLearningPortalContext(options);
            context.Пользователи.AddRange(
                new Пользователи { Код_пользователя = 1, ФИО = "Пользователь 1" },
                new Пользователи { Код_пользователя = 2, ФИО = "Пользователь 2" },
                new Пользователи { Код_пользователя = 3, ФИО = "Пользователь 3" }
            );
            context.SaveChanges();
            var controller = new UsersListController(context);

            // Act
            var result = await controller.Index(null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Пользователи>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Пользователи>>(result.ViewData.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task Index_ReturnsFilteredViewResult_WithListOfUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new PythonLearningPortalContext(options);
            context.Пользователи.AddRange(
                new Пользователи { Код_пользователя = 1, ФИО = "Пользователь 1" },
                new Пользователи { Код_пользователя = 2, ФИО = "Пользователь 2" },
                new Пользователи { Код_пользователя = 3, ФИО = "Пользователь 3" }
            );
            context.SaveChanges();
            var controller = new UsersListController(context);

            // Act
            var result = await controller.Index("2") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Пользователи>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Пользователи>>(result.ViewData.Model);
            Assert.Single(model);
            Assert.Equal("Пользователь 2", model.First().ФИО);
        }
    }
}
