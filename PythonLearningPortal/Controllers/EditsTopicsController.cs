using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PythonLearningPortal.DataContext;
using PythonLearningPortal.Models;

namespace PythonLearningPortal.Controllers
{
    public class EditsTopicsController : Controller
    {
        private readonly PythonLearningPortalContext _context;

        public EditsTopicsController(PythonLearningPortalContext context)
        {
            _context = context;
        }

        // GET: EditsTopics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Темы.ToListAsync());
        }

        // GET: EditsTopics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var темы = await _context.Темы
                .FirstOrDefaultAsync(m => m.Код_темы == id);
            if (темы == null)
            {
                return NotFound();
            }

            return View(темы);
        }

        // GET: EditsTopics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var темы = await _context.Темы.FindAsync(id);
            if (темы == null)
            {
                return NotFound();
            }
            return View(темы);
        }

        // POST: EditsTopics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_темы,Название_темы")] Темы темы)
        {
            if (id != темы.Код_темы)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(темы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ТемыExists(темы.Код_темы))
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
            return View(темы);
        }

        // GET: EditsTopics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var темы = await _context.Темы
                .FirstOrDefaultAsync(m => m.Код_темы == id);
            if (темы == null)
            {
                return NotFound();
            }

            return View(темы);
        }

        // POST: EditsTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var темы = await _context.Темы.FindAsync(id);
            if (темы != null)
            {
                _context.Темы.Remove(темы);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ТемыExists(int id)
        {
            return _context.Темы.Any(e => e.Код_темы == id);
        }
    }
}
