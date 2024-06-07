using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Tests
{
    public class AddTopicControllerTests
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
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddTopicController(context);

                // Act
                var result = controller.Index();

                // Assert
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Index_Post_RedirectsToHomeIndex_WhenModelIsValid()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddTopicController(context);
                var model = new AddTopicViewModel
                {
                    TopicCode = 1,
                    NameTopic = "New Topic"
                };

                // Act
                var result = await controller.Index(model);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Equal("Home", redirectToActionResult.ControllerName);

                // Verify the topic was added to the database
                var topic = await context.Темы.FirstOrDefaultAsync(t => t.Код_темы == model.TopicCode);
                Assert.NotNull(topic);
                Assert.Equal(model.NameTopic, topic.Название_темы);
            }
        }

        [Fact]
        public async Task Index_Post_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddTopicController(context);
                controller.ModelState.AddModelError("NameTopic", "Required");
                var model = new AddTopicViewModel
                {
                    TopicCode = 1,
                    NameTopic = ""
                };

                // Act
                var result = await controller.Index(model);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var returnedModel = Assert.IsType<AddTopicViewModel>(viewResult.Model);
                Assert.Equal(model, returnedModel);
            }
        }

        [Fact]
        public async Task Index_Post_ReturnsViewResult_WithErrorMessage_OnDbUpdateException()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var mockSet = new Mock<DbSet<Темы>>();
                mockSet.Setup(m => m.Add(It.IsAny<Темы>())).Throws(new DbUpdateException());

                var mockContext = new Mock<PythonLearningPortalContext>(options);
                mockContext.Setup(c => c.Темы).Returns(mockSet.Object);

                var controller = new AddTopicController(mockContext.Object);
                var model = new AddTopicViewModel
                {
                    TopicCode = 1,
                    NameTopic = "New Topic"
                };

                // Act
                var result = await controller.Index(model);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var returnedModel = Assert.IsType<AddTopicViewModel>(viewResult.Model);
                Assert.Equal(model, returnedModel);
                Assert.True(controller.ModelState.ContainsKey(""));
                Assert.Equal("Произошла ошибка при добавлении новой темы. Пожалуйста, попробуйте еще раз.",
                    controller.ModelState[""].Errors[0].ErrorMessage);
            }
        }
    }
}
