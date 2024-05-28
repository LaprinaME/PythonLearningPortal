using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Linq;
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
        public async Task<IActionResult> Index(string filter, int? length)
        {
            var accounts = _context.Аккаунты.Include(a => a.Роли).AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                accounts = accounts.Where(a => a.Логин.Contains(filter));
            }

            if (length.HasValue)
            {
                accounts = accounts.Where(a => a.Логин.Length <= length.Value);
            }

            return View(await accounts.ToListAsync());
        }
    }
}
