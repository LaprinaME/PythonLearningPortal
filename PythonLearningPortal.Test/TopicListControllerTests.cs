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
    public class TopicListControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTopics()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new PythonLearningPortalContext(options);
            context.Темы.AddRange(
                new Темы { Код_темы = 1, Название_темы = "Тема 1" },
                new Темы { Код_темы = 2, Название_темы = "Тема 2" },
                new Темы { Код_темы = 3, Название_темы = "Тема 3" }
            );
            context.SaveChanges();
            var controller = new TopicListController(context);

            // Act
            var result = await controller.Index(null, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Темы>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Темы>>(result.ViewData.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task Index_ReturnsFilteredViewResult_WithListOfTopics()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new PythonLearningPortalContext(options);
            context.Темы.AddRange(
                new Темы { Код_темы = 1, Название_темы = "Тема 1" },
                new Темы { Код_темы = 2, Название_темы = "Тема 2" },
                new Темы { Код_темы = 3, Название_темы = "Тема 3" }
            );
            context.SaveChanges();
            var controller = new TopicListController(context);

            // Act
            var result = await controller.Index("1", null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Темы>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Темы>>(result.ViewData.Model);
            Assert.Single(model);
            Assert.Equal("Тема 1", model.First().Название_темы);
        }

        [Fact]
        public async Task Index_ReturnsSortedViewResult_WithListOfTopics()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new PythonLearningPortalContext(options);
            context.Темы.AddRange(
                new Темы { Код_темы = 1, Название_темы = "Тема 1" },
                new Темы { Код_темы = 2, Название_темы = "Тема 2" },
                new Темы { Код_темы = 3, Название_темы = "Тема 3" }
            );
            context.SaveChanges();
            var controller = new TopicListController(context);

            // Act
            var result = await controller.Index(null, "name_desc") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Темы>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Темы>>(result.ViewData.Model);
            Assert.Equal("Тема 3", model.First().Название_темы);
        }
    }
}
