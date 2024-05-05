using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.ViewModels
{
    // Модель для авторизации
    public class LoginViewModel
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}

