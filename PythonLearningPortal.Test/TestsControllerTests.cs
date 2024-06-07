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
    public class TestsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithAllTests()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PythonLearningPortalContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize the database with abstract test data
            using (var context = new PythonLearningPortalContext(options))
            {
                context.Тесты.AddRange(GetTestAbstractData());
                context.SaveChanges();
            }

            // Act
            using (var context = new PythonLearningPortalContext(options))
            {
                var controller = new TestsController(context);
                var result = await controller.Index() as ViewResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<Тесты>>(result.ViewData.Model);
                var model = Assert.IsAssignableFrom<List<Тесты>>(result.ViewData.Model);
                Assert.Equal(GetTestAbstractData().Count, model.Count);
            }
        }

        // Method to generate abstract test data
        private List<Тесты> GetTestAbstractData()
        {
            return new List<Тесты>
            {
                new Тесты { Код_теста = 1, Название_теста = "Abstract Test 1" },
                new Тесты { Код_теста = 2, Название_теста = "Abstract Test 2" },
                new Тесты { Код_теста = 3, Название_теста = "Abstract Test 3" }
            };
        }
    }
}
