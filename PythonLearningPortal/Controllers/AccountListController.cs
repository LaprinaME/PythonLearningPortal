using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class AccountListController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public AccountListController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: AccountList
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Аккаунты.Include(a => a.Роли).ToListAsync();
            return View(accounts);
        }
    }
}
