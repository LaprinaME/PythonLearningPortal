using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PythonLearningPortal.Test
{
    public class TestsListControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithListOfTests()
        {
            // Arrange
            var controller = new TestsListController();

            // Act
            var result = controller.Index(null, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Тесты>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Тесты>>(result.ViewData.Model);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public void Index_ReturnsFilteredViewResult_WithListOfTests()
        {
            // Arrange
            var controller = new TestsListController();

            // Act
            var result = controller.Index("1", null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Тесты>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Тесты>>(result.ViewData.Model);
            Assert.Single(model);
            Assert.Equal("Тест 1", model.First().Название_теста);
        }

        [Fact]
        public void Index_ReturnsSortedViewResult_WithListOfTests()
        {
            // Arrange
            var controller = new TestsListController();

            // Act
            var result = controller.Index(null, "name_desc") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Тесты>>(result.ViewData.Model);
            var model = Assert.IsAssignableFrom<List<Тесты>>(result.ViewData.Model);
            Assert.Equal("Тест 3", model.First().Название_теста);
        }
    }
}
