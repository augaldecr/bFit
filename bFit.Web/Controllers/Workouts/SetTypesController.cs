using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bFit.WEB.Data.Entities.Workouts;
using bFit.Web.Data;

namespace bFit.Web.Controllers.Workouts
{
    public class SetTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SetTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SetTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SetTypes.ToListAsync());
        }

        // GET: SetTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setType = await _context.SetTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setType == null)
            {
                return NotFound();
            }

            return View(setType);
        }

        // GET: SetTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SetTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SetType setType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(setType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(setType);
        }

        // GET: SetTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setType = await _context.SetTypes.FindAsync(id);
            if (setType == null)
            {
                return NotFound();
            }
            return View(setType);
        }

        // POST: SetTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SetType setType)
        {
            if (id != setType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(setType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SetTypeExists(setType.Id))
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
            return View(setType);
        }

        // GET: SetTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setType = await _context.SetTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setType == null)
            {
                return NotFound();
            }

            return View(setType);
        }

        // POST: SetTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var setType = await _context.SetTypes.FindAsync(id);
            _context.SetTypes.Remove(setType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SetTypeExists(int id)
        {
            return _context.SetTypes.Any(e => e.Id == id);
        }
    }
}
