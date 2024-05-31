using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;

namespace PythonLearningPortal.Test
{
    public class SubtopicListControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfSubtopics()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Подтемы.AddRange(
                    new Подтемы { Код_подтемы = 1, Название_подтемы = "Subtopic 1" },
                    new Подтемы { Код_подтемы = 2, Название_подтемы = "Subtopic 2" },
                    new Подтемы { Код_подтемы = 3, Название_подтемы = "Subtopic 3" }
                );
                await context.SaveChangesAsync();
            }

            // Use a clean instance of the context to run the test
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new SubtopicListController(context);

                // Act
                var result = await controller.Index(null);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Подтемы>>(viewResult.ViewData.Model);
                Assert.Equal(3, model.Count);
            }
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithFilteredListOfSubtopics()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Подтемы.AddRange(
                    new Подтемы { Код_подтемы = 1, Название_подтемы = "Subtopic 1" },
                    new Подтемы { Код_подтемы = 2, Название_подтемы = "Subtopic 2" },
                    new Подтемы { Код_подтемы = 3, Название_подтемы = "Subtopic 3" }
                );
                await context.SaveChangesAsync();
            }

            // Use a clean instance of the context to run the test
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new SubtopicListController(context);

                // Act
                var result = await controller.Index("Subtopic 2");

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Подтемы>>(viewResult.ViewData.Model);
                Assert.Single(model);
                Assert.Equal("Subtopic 2", model.First().Название_подтемы);
            }
        }
    }
}
