using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.ViewModels
{
    public class EditTopicViewModel
    {
        public int TopicCode { get; set; }

        [Required(ErrorMessage = "Поле 'Название темы' обязательно для заполнения.")]
        public string NameTopic { get; set; }
    }
}
