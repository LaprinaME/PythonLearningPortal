using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class DeleteUsersController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public DeleteUsersController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        [Route("DeleteUsers")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Пользователи.ToListAsync();
            var viewModel = new List<DeleteUsersViewModel>();

            foreach (var user in users)
            {
                viewModel.Add(new DeleteUsersViewModel
                {
                    Код_пользователя = user.Код_пользователя,
                    ФИО = user.ФИО
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int userId)
        {
            var user = await _context.Пользователи.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            _context.Пользователи.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
