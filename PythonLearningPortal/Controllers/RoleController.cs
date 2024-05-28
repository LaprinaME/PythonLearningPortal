using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class RoleController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public RoleController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: RoleList
        public async Task<IActionResult> Index(string filter)
        {
            var rolesQuery = _context.Роли.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                rolesQuery = rolesQuery.Where(r => r.Название_роли.Contains(filter));
            }

            var roles = await rolesQuery.ToListAsync();
            return View(roles);
        }
    }
}
