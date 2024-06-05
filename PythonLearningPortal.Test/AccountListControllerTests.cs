using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.Controllers;
using PythonLearningPortal.Models;
using PythonLearningPortal.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PythonLearningPortal.Test
{
    public class AccountListControllerTests
    {
        [Fact]
        public async Task Index_ReturnsFilteredAccount()
        {
            var mockContext = new Mock<PythonLearningPortalContext>();
            var mockDbSet = new Mock<DbSet<Аккаунты>>();

            var data = new List<Аккаунты>
            {
                new Аккаунты { Логин = "user1", Пароль = "password1" }
            }.AsQueryable();

            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(c => c.Аккаунты).Returns(mockDbSet.Object);

            var controller = new AccountListController(mockContext.Object);

            var result = await controller.Index("user1", null);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Аккаунты>>(viewResult.Model);
            Assert.Single(model);
            Assert.Contains(model, a => a.Логин == "user1");
        }

        [Fact]
        public async Task Index_ReturnsAccountWithLimitedLength()
        {
            var mockContext = new Mock<PythonLearningPortalContext>();
            var mockDbSet = new Mock<DbSet<Аккаунты>>();

            var data = new List<Аккаунты>
            {
                new Аккаунты { Логин = "user1", Пароль = "password1" }
            }.AsQueryable();

            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Аккаунты>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(c => c.Аккаунты).Returns(mockDbSet.Object);

            var controller = new AccountListController(mockContext.Object);

            var result = await controller.Index(null, 10);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Аккаунты>>(viewResult.Model);
            Assert.Single(model);
            Assert.Contains(model, a => a.Логин == "user1");
        }
    }
}
