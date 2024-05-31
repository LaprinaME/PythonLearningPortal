using Xunit;
using Microsoft.AspNetCore.Mvc;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.ViewModels;

namespace PythonLearningPortal.Test
{
    public class RegistrationControllerTests
    {
        [Fact]
        public void Register_GET_ReturnsViewResult()
        {
            // Arrange
            var controller = new RegistrationController();

            // Act
            var result = controller.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Register_POST_RedirectsToHomeIndex()
        {
            // Arrange
            var controller = new RegistrationController();
            var model = new RegisterViewModel();

            // Act
            var result = controller.Register(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }
    }
}
