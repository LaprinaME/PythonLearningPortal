using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;

namespace PythonLearningPortal.Controllers
{
    public class SubtopicsController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public SubtopicsController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: Subtopics
        public async Task<IActionResult> Index()
        {
            var pythonLearningPortalContext = _context.Подтемы.Include(p => p.Темы);
            return View(await pythonLearningPortalContext.ToListAsync());
        }

        // GET: Subtopics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtopic = await _context.Подтемы
                .Include(p => p.Темы)
                .FirstOrDefaultAsync(m => m.Код_подтемы == id);
            if (subtopic == null)
            {
                return NotFound();
            }

            return View(subtopic);
        }

        // GET: Subtopics/Create
        public IActionResult Create()
        {
            ViewData["Код_темы"] = new SelectList(_context.Темы, "Код_темы", "Название_темы");
            return View();
        }

        // POST: Subtopics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_подтемы,Название_подтемы,Код_темы")] Подтемы subtopic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subtopic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_темы"] = new SelectList(_context.Темы, "Код_темы", "Название_темы", subtopic.Код_темы);
            return View(subtopic);
        }

        // GET: Subtopics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtopic = await _context.Подтемы.FindAsync(id);
            if (subtopic == null)
            {
                return NotFound();
            }
            ViewData["Код_темы"] = new SelectList(_context.Темы, "Код_темы", "Название_темы", subtopic.Код_темы);
            return View(subtopic);
        }

        // POST: Subtopics/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_подтемы,Название_подтемы,Код_темы")] Подтемы subtopic)
        {
            if (id != subtopic.Код_подтемы)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subtopic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubtopicExists(subtopic.Код_подтемы))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_темы"] = new SelectList(_context.Темы, "Код_темы", "Название_темы", subtopic.Код_темы);
            return View(subtopic);
        }

        // GET: Subtopics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtopic = await _context.Подтемы
                .Include(p => p.Темы)
                .FirstOrDefaultAsync(m => m.Код_подтемы == id);
            if (subtopic == null)
            {
                return NotFound();
            }

            return View(subtopic);
        }

        // POST: Subtopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subtopic = await _context.Подтемы.FindAsync(id);
            if (subtopic != null)
            {
                _context.Подтемы.Remove(subtopic);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubtopicExists(int id)
        {
            return _context.Подтемы.Any(e => e.Код_подтемы == id);
        }
    }
}
