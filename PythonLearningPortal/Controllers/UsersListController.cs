using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Linq;
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
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var users = from u in _context.Пользователи.Include(u => u.Аккаунты)
                        select u;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.ФИО.Contains(searchString));
            }

            return View(await users.ToListAsync());
        }
    }
}
