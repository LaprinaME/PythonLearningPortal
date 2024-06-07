using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Tests
{
    public class EditSubtopicControllerTests
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
        public async Task Index_Get_ReturnsViewResult_WithListOfTopicsAndSubtopics()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Темы.Add(new Темы { Код_темы = 1, Название_темы = "Topic1" });
                context.Подтемы.Add(new Подтемы { Код_подтемы = 1, Название_подтемы = "Subtopic1", Код_темы = 1 });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new EditSubtopicController(context);

                // Act
                var result = await controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<EditSubtopicViewModel>(viewResult.Model);
                Assert.Single(model.Topics);
                Assert.Single(model.Subtopics);
            }
        }

        [Fact]
        public async Task Index_Get_WithSubtopicId_ReturnsViewResult_WithCorrectSubtopic()
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
                var controller = new EditSubtopicController(context);

                // Act
                var result = await controller.Index(1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<EditSubtopicViewModel>(viewResult.Model);
                Assert.Equal(1, model.SubtopicCode);
                Assert.Equal("Subtopic1", model.NameSubtopic);
            }
        }

        [Fact]
        public async Task Index_Get_WithInvalidSubtopicId_ReturnsNotFound()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new EditSubtopicController(context);

                // Act
                var result = await controller.Index(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Index_Post_UpdatesSubtopicAndRedirects()
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
                var controller = new EditSubtopicController(context);
                var viewModel = new EditSubtopicViewModel
                {
                    SubtopicCode = 1,
                    NameSubtopic = "UpdatedSubtopic",
                    TopicCode = 1
                };

                // Act
                var result = await controller.Index(viewModel);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Equal("Home", redirectToActionResult.ControllerName);

                var updatedSubtopic = await context.Подтемы.FindAsync(1);
                Assert.Equal("UpdatedSubtopic", updatedSubtopic.Название_подтемы);
            }
        }

        [Fact]
        public async Task Index_Post_WithInvalidModelState_ReturnsViewResult_WithViewModel()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new EditSubtopicController(context);
                controller.ModelState.AddModelError("NameSubtopic", "Required");
                var viewModel = new EditSubtopicViewModel
                {
                    SubtopicCode = 1,
                    NameSubtopic = "InvalidSubtopic",
                    TopicCode = 1
                };

                // Act
                var result = await controller.Index(viewModel);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<EditSubtopicViewModel>(viewResult.Model);
                Assert.Equal(viewModel, model);
            }
        }
    }
}
