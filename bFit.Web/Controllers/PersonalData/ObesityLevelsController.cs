using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bFit.WEB.Data.Entities.PersonalData;
using bFit.Web.Data;

namespace bFit.Web.Controllers.PersonalData
{
    public class ObesityLevelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObesityLevelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ObesityLevels
        public async Task<IActionResult> Index()
        {
            return View(await _context.ObesityLevels.ToListAsync());
        }

        // GET: ObesityLevels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obesityLevel = await _context.ObesityLevels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obesityLevel == null)
            {
                return NotFound();
            }

            return View(obesityLevel);
        }

        // GET: ObesityLevels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ObesityLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ObesityLevel obesityLevel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(obesityLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obesityLevel);
        }

        // GET: ObesityLevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obesityLevel = await _context.ObesityLevels.FindAsync(id);
            if (obesityLevel == null)
            {
                return NotFound();
            }
            return View(obesityLevel);
        }

        // POST: ObesityLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ObesityLevel obesityLevel)
        {
            if (id != obesityLevel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obesityLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObesityLevelExists(obesityLevel.Id))
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
            return View(obesityLevel);
        }

        // GET: ObesityLevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obesityLevel = await _context.ObesityLevels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obesityLevel == null)
            {
                return NotFound();
            }

            return View(obesityLevel);
        }

        // POST: ObesityLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obesityLevel = await _context.ObesityLevels.FindAsync(id);
            _context.ObesityLevels.Remove(obesityLevel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObesityLevelExists(int id)
        {
            return _context.ObesityLevels.Any(e => e.Id == id);
        }
    }
}
