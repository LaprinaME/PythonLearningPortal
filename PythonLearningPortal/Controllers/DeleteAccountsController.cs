using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;
using PythonLearningPortal.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PythonLearningPortal.Controllers
{
    public class DeleteAccountsController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public DeleteAccountsController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        [Route("DeleteAccounts")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Аккаунты.ToListAsync();
            var viewModel = new List<AccountViewModel>();

            foreach (var account in accounts)
            {
                viewModel.Add(new AccountViewModel
                {
                    Код_аккаунта = account.Код_аккаунта,
                    Логин = account.Логин
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int accountId)
        {
            var account = await _context.Аккаунты.FindAsync(accountId);

            if (account == null)
            {
                return NotFound();
            }

            _context.Аккаунты.Remove(account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
