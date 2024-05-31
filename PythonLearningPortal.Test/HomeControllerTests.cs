using Xunit; // ����������� ���������� xUnit ��� ��������� � ���������� ������.
using Microsoft.AspNetCore.Mvc; // ����������� ���������� ��� ������ � ������������� � ���������� � ASP.NET Core.
using PythonLearningPortal.Controllers; // ����������� ������������ ����, � ������� ��������� HomeController.

namespace PythonLearningPortal.Test // ����������� ������ ������������ ���� ��� ������.
{
    public class HomeControllerTests // ���������� ������ ��� ������ ����������� HomeController.
    {
        [Fact] // �������, �����������, ��� ����� �������� �������� � ������ ����������� ��� ���� xUnit.
        public void Index_ReturnsViewResult() // ���������� ��������� ������, ������������ ����� Index �����������.
        {
            // Arrange
            var controller = new HomeController(); // �������� ���������� HomeController ��� ������������.

            // Act
            var result = controller.Index(); // ����� ������ Index ����������� � ���������� ����������.

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // ��������, ��� ��������� �������� �������� ���� ViewResult.
            Assert.NotNull(viewResult); // ��������, ��� ��������� �� ����� null.
        }
    }
}
