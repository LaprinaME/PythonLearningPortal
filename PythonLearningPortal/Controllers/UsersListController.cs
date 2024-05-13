using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class UsersListController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public UsersListController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: UsersList
        public async Task<IActionResult> Index()
        {
            var users = await _context.Пользователи.Include(u => u.Аккаунты).ToListAsync();
            return View(users);
        }
    }
}
