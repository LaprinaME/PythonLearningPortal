using System.Collections.Generic;
using System.Linq;

namespace PythonLearningPortal.ViewModels
{
    public class TopicViewModel
    {
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }
        public string Description { get; set; } // Добавлено поле описания
        public List<SubtopicViewModel> Subtopics { get; set; } // Добавлено свойство для подтем
    }

    public class SubtopicViewModel
    {
        public int SubtopicId { get; set; }
        public string SubtopicTitle { get; set; }
        public int TopicId { get; set; }
        public string Description { get; set; } // Добавлено поле описания
    }
}
