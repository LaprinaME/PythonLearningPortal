using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.ViewModels;
using PythonLearningPortal.Models;

namespace PythonLearningPortal.Controllers
{
    public class TopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public TopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Получаем все темы из базы данных
            List<TopicViewModel> topicViewModels = _context.Темы.Select(topic => new TopicViewModel
            {
                TopicId = topic.Код_темы,
                TopicTitle = topic.Название_темы,
                Subtopics = _context.Подтемы
                            .Where(subtopic => subtopic.Код_темы == topic.Код_темы)
                            .Select(subtopic => new SubtopicViewModel
                            {
                                SubtopicId = subtopic.Код_подтемы,
                                SubtopicTitle = subtopic.Название_подтемы,
                                TopicId = topic.Код_темы
                            }).ToList()
            }).ToList();

            return View(topicViewModels);
        }
    }
}
