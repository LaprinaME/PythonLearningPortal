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
    public class DeleteTopicControllerTests
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
        public async Task Index_ReturnsViewResult_WithListOfTopics()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Темы.Add(new Темы { Код_темы = 1, Название_темы = "Topic1" });
                context.Темы.Add(new Темы { Код_темы = 2, Название_темы = "Topic2" });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteTopicController(context);

                // Act
                var result = await controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<TopicViewModel>>(viewResult.Model);
                Assert.Equal(2, model.Count);
                Assert.Equal("Topic1", model[0].TopicTitle);
                Assert.Equal("Topic2", model[1].TopicTitle);
            }
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTopicDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteTopicController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Delete_RedirectsToIndex_WhenTopicIsDeleted()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Темы.Add(new Темы { Код_темы = 1, Название_темы = "Topic1" });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new DeleteTopicController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);

                var topic = await context.Темы.FindAsync(1);
                Assert.Null(topic);
            }
        }
    }
}
