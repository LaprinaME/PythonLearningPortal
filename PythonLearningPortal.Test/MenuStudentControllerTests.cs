using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.Controllers;
using Xunit;

namespace PythonLearningPortal.Test
{
    public class MenuStudentControllerTests
    {
        [Fact]
        public void Index_Get_ReturnsViewResult()
        {
            // Arrange
            var controller = new MenuStudentController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Index_Post_RedirectsToAction_WhenPageIsProvided()
        {
            // Arrange
            var controller = new MenuStudentController();
            string page = "/SomePage";

            // Act
            var result = controller.Index(page) as RedirectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(page, result.Url);
        }

        [Fact]
        public void Index_Post_RedirectsToAction_WhenPageIsNotProvided()
        {
            // Arrange
            var controller = new MenuStudentController();

            // Act
            var result = controller.Index(null) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
