using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PythonLearningPortal.Test
{
    public class TestsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTests()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new PythonLearningPortalContext(options);
            context.Тесты.AddRange(
                new Тесты { Код_теста = 1, Название_теста = "Тест 1" },
                new Тесты { Код_теста = 2, Название_теста = "Тест 2" },
                new Тесты { Код_теста = 3, Название_теста = "Тест 3" }
            );
            context.SaveChanges();
            var controller = new TestsController(context);

            // Act
            var result = await controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Тесты>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Тесты>>(result.ViewData.Model);
            Assert.Equal(3, model.Count);
        }
    }
}
