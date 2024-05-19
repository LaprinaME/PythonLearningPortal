using System.ComponentModel.DataAnnotations;

namespace PythonLearningPortal.ViewModels
{
    public class DeleteSubtopicViewModel
    {
        public int SubtopicCode { get; set; }

        [Display(Name = "Название подтемы")]
        public string NameSubtopic { get; set; }
        public int TopicCode { get; set; }
    }
}
