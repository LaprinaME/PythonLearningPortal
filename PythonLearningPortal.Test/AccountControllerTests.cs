using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Models;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.ViewModels;
using PythonLearningPortal.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

public class AccountControllerTests
{
    private DbContextOptions<PythonLearningPortalContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<PythonLearningPortalContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task Login_UserExists_RedirectsToCorrectPage()
    {
        // Arrange
        var options = GetDbContextOptions();

        // Insert seed data into the database using one instance of the context
        using (var context = new PythonLearningPortalContext(options))
        {
            var role = new Роли { Код_роли = 1, Название_роли = "Student" };
            context.Роли.Add(role);
            context.Аккаунты.Add(new Аккаунты { Логин = "testuser", Пароль = "password", Роли = role });
            context.SaveChanges();
        }

        using (var context = new PythonLearningPortalContext(options))
        {
            var httpContext = new Mock<HttpContext>();
            var authService = new Mock<IAuthenticationService>();
            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var urlHelper = new Mock<IUrlHelper>();

            httpContext.Setup(x => x.RequestServices.GetService(typeof(IAuthenticationService))).Returns(authService.Object);
            httpContext.Setup(x => x.RequestServices.GetService(typeof(IUrlHelperFactory))).Returns(urlHelperFactory.Object);
            urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            urlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns("/MenuStudent/Index");

            var controller = new AccountController(context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext.Object
                },
                Url = urlHelper.Object
            };

            var model = new LoginViewModel { Login = "testuser", Password = "password" };

            // Act
            var result = await controller.Login(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("MenuStudent", redirectToActionResult.ControllerName);
        }
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsViewWithModelError()
    {
        // Arrange
        var options = GetDbContextOptions();

        using (var context = new PythonLearningPortalContext(options))
        {
            var controller = new AccountController(context);
            var model = new LoginViewModel { Login = "nonexistentuser", Password = "wrongpassword" };

            // Act
            var result = await controller.Login(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(model, viewResult.Model);
            Assert.False(controller.ModelState.IsValid);
            Assert.Contains(controller.ModelState, kvp => kvp.Value.Errors.Any(e => e.ErrorMessage == "Неверный логин или пароль"));
        }
    }
}
