using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.ViewModels
{
    public class EditSubtopicViewModel
    {
        public int SubtopicCode { get; set; }
        public int TopicCode { get; set; }

        [Required(ErrorMessage = "Поле 'Название подтемы' обязательно для заполнения.")]
        public string NameSubtopic { get; set; }
    }
}
