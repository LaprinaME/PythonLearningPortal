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
    public class DeleteSubtopicControllerTests
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
        public async Task Index_ReturnsViewResult_WithListOfSubtopics()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Подтемы.Add(new Подтемы { Код_подтемы = 1, Название_подтемы = "Subtopic1", Код_темы = 1 });
                context.Подтемы.Add(new Подтемы { Код_подтемы = 2, Название_подтемы = "Subtopic2", Код_темы = 2 });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteSubtopicController(context);

                // Act
                var result = await controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<DeleteSubtopicViewModel>>(viewResult.Model);
                Assert.Equal(2, model.Count);
                Assert.Equal("Subtopic1", model[0].NameSubtopic);
                Assert.Equal("Subtopic2", model[1].NameSubtopic);
            }
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenSubtopicDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteSubtopicController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_RedirectsToIndex_WhenSubtopicIsDeleted()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Подтемы.Add(new Подтемы { Код_подтемы = 1, Название_подтемы = "Subtopic1", Код_темы = 1 });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteSubtopicController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);

                var subtopic = await context.Подтемы.FindAsync(1);
                Assert.Null(subtopic);
            }
        }
    }
}
