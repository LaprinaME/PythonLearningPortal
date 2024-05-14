using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.ViewModels
{
    public class AddSubtopicViewModel
    {
        public int TopicId { get; set; }
        public int SubtopicId { get; set; }

        [Required(ErrorMessage = "Поле 'Название подтемы' обязательно для заполнения.")]
        [StringLength(255, ErrorMessage = "Название подтемы должно содержать не более 255 символов.")]
        public string SubtopicName { get; set; }
    }
}
