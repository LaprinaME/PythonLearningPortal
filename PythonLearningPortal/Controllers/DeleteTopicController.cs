using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class DeleteTopicController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public DeleteTopicController(PythonLearningPortalContext context)
        {
            _context = context;
        }
        [Route("DeleteTopic")]

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var topics = await _context.Темы.ToListAsync();
            var viewModel = new List<TopicViewModel>();

            foreach (var topic in topics)
            {
                viewModel.Add(new TopicViewModel
                {
                    TopicId = topic.Код_темы,
                    TopicTitle = topic.Название_темы
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int topicId)
        {
            var topic = await _context.Темы.FindAsync(topicId);

            if (topic == null)
            {
                return NotFound();
            }

            _context.Темы.Remove(topic);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
