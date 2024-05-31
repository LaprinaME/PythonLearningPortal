using Xunit;
using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.Controllers;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;

namespace PythonLearningPortal.Test
{
    public class RoleControllerTests
    {
        [Fact]
        public async Task Index_GET_ReturnsViewResultWithRoles()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Insert test data into the in-memory database
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Роли.AddRange(
                    new Роли { Название_роли = "Role 1" },
                    new Роли { Название_роли = "Role 2" },
                    new Роли { Название_роли = "Role 3" }
                );
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new RoleController(context);
                var result = await controller.Index(null) as ViewResult;
                var model = result.Model as List<Роли>;

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(model);
                Assert.Equal(3, model.Count); // Check if all roles are retrieved
            }
        }

        [Fact]
        public async Task Index_GET_ReturnsFilteredViewResultWithRoles()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Insert test data into the in-memory database
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Роли.AddRange(
                    new Роли { Название_роли = "Role 1" },
                    new Роли { Название_роли = "Role 2" },
                    new Роли { Название_роли = "Role 3" }
                );
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new RoleController(context);
                var result = await controller.Index("Role 1") as ViewResult;
                var model = result.Model as List<Роли>;

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(model);
                Assert.Single(model); // Check if only one role is retrieved
                Assert.Equal("Role 1", model.First().Название_роли); // Check if the correct role is retrieved
            }
        }
    }
}
