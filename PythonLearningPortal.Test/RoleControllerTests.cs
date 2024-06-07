using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Tests
{
    public class RoleControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfRoles()
        {
            // Arrange
            // Use an in-memory database provider for testing
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize the database with test data
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Роли.AddRange(
                    new Роли { Код_роли = 1, Название_роли = "Role 1" },
                    new Роли { Код_роли = 2, Название_роли = "Role 2" },
                    new Роли { Код_роли = 3, Название_роли = "Role 3" }
                );
                context.SaveChanges();
            }

            // Act
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new RoleController(context);
                var result = await controller.Index(null) as ViewResult;

                // Assert
                Assert.NotNull(result);
                var model = result.Model as List<Роли>;
                Assert.NotNull(model);
                Assert.Equal(3, model.Count);
            }
        }

        [Fact]
        public async Task Index_ReturnsFilteredViewResult_WithFilteredListOfRoles()
        {
            // Arrange
            // Use an in-memory database provider for testing
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize the database with test data
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Роли.AddRange(
                    new Роли { Код_роли = 1, Название_роли = "Role 1" },
                    new Роли { Код_роли = 2, Название_роли = "Role 2" },
                    new Роли { Код_роли = 3, Название_роли = "Role 3" }
                );
                context.SaveChanges();
            }

            // Act
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new RoleController(context);
                var result = await controller.Index("Role 1") as ViewResult;

                // Assert
                Assert.NotNull(result);
                var model = result.Model as List<Роли>;
                Assert.NotNull(model);
                Assert.Single(model);
                Assert.Equal("Role 1", model.First().Название_роли);
            }
        }
    }
}
