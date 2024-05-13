using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
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
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Роли.ToListAsync();
            return View(roles);
        }
    }
}
