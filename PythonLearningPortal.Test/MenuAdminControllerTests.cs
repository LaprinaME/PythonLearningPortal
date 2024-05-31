using Xunit;
using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.Controllers;

namespace PythonLearningPortal.Test
{
    public class MenuAdminControllerTests
    {
        [Fact]
        public void Index_GET_ReturnsViewResult()
        {
            // Arrange
            var controller = new MenuAdminController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Theory]
        [InlineData("/SomePage")]
        [InlineData("/AnotherPage")]
        public void Index_POST_RedirectsToPage(string page)
        {
            // Arrange
            var controller = new MenuAdminController();

            // Act
            var result = controller.Index(page) as RedirectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(page, result.Url);
        }

        [Fact]
        public void Index_POST_NoPage_RedirectsToIndex()
        {
            // Arrange
            var controller = new MenuAdminController();

            // Act
            var result = controller.Index(null) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}

