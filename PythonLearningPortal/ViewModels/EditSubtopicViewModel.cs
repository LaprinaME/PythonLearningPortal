using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.ViewModels
{
    public class EditSubtopicViewModel
    {
        public int SubtopicCode { get; set; }
        public int TopicCode { get; set; }

        [Required(ErrorMessage = "Поле 'Название подтемы' обязательно для заполнения.")]
        public string NameSubtopic { get; set; }

        // Свойство для списка тем
        public List<TopicViewModel> Topics { get; set; }
        public List<SubtopicViewModel> Subtopics { get; set; }
    }
}
