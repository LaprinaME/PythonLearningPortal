﻿using Microsoft.AspNetCore.Mvc;
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
    public class AddSubtopicControllerTests
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
        public async Task Index_ReturnsViewResult_WhenTopicIdIsNull()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddSubtopicController(context);

                // Act
                var result = await controller.Index(null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithViewModel_WhenTopicExists()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Темы.Add(new Темы { Код_темы = 1, Название_темы = "Test Topic" });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddSubtopicController(context);

                // Act
                var result = await controller.Index(1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<AddSubtopicViewModel>(viewResult.Model);
                Assert.Equal(1, model.TopicId);
            }
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenTopicDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddSubtopicController(context);

                // Act
                var result = await controller.Index(99);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task Create_RedirectsToHomeIndex_WhenModelIsValid()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                context.Темы.Add(new Темы { Код_темы = 1, Название_темы = "Test Topic" });
                context.SaveChanges();
            }

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddSubtopicController(context);

                var viewModel = new AddSubtopicViewModel
                {
                    TopicId = 1,
                    SubtopicName = "New Subtopic",
                    SubtopicId = 0 // Assuming new subtopic
                };

                // Act
                var result = await controller.Create(viewModel);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Equal("Home", redirectToActionResult.ControllerName);
            }
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WithSameViewModel_WhenModelIsInvalid()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new AddSubtopicController(context);
                controller.ModelState.AddModelError("SubtopicName", "Required");

                var viewModel = new AddSubtopicViewModel
                {
                    TopicId = 1,
                    SubtopicName = "",
                    SubtopicId = 0 // Assuming new subtopic
                };

                // Act
                var result = await controller.Create(viewModel);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<AddSubtopicViewModel>(viewResult.Model);
                Assert.Equal(viewModel, model);
            }
        }
    }
}
