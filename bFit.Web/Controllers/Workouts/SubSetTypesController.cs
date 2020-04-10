using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bFit.Web.Data;
using bFit.Web.Data.Entities.Workouts;

namespace bFit.Web.Controllers.Workouts
{
    public class SubSetTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubSetTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.SubSetTypes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSetType = await _context.SubSetTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subSetType == null)
            {
                return NotFound();
            }

            return View(subSetType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SubSetType subSetType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subSetType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subSetType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSetType = await _context.SubSetTypes.FindAsync(id);
            if (subSetType == null)
            {
                return NotFound();
            }
            return View(subSetType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SubSetType subSetType)
        {
            if (id != subSetType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subSetType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubSetTypeExists(subSetType.Id))
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
            return View(subSetType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSetType = await _context.SubSetTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subSetType == null)
            {
                return NotFound();
            }

            return View(subSetType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subSetType = await _context.SubSetTypes.FindAsync(id);
            _context.SubSetTypes.Remove(subSetType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubSetTypeExists(int id)
        {
            return _context.SubSetTypes.Any(e => e.Id == id);
        }
    }
}
