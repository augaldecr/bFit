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
    public class SubSetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubSetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubSets
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubSets.ToListAsync());
        }

        // GET: SubSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSet = await _context.SubSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subSet == null)
            {
                return NotFound();
            }

            return View(subSet);
        }

        // GET: SubSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubSets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,PositiveTime,NegativeTime,Id,Name")] SubSet subSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subSet);
        }

        // GET: SubSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSet = await _context.SubSets.FindAsync(id);
            if (subSet == null)
            {
                return NotFound();
            }
            return View(subSet);
        }

        // POST: SubSets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Quantity,PositiveTime,NegativeTime,Id,Name")] SubSet subSet)
        {
            if (id != subSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubSetExists(subSet.Id))
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
            return View(subSet);
        }

        // GET: SubSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSet = await _context.SubSets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subSet == null)
            {
                return NotFound();
            }

            return View(subSet);
        }

        // POST: SubSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subSet = await _context.SubSets.FindAsync(id);
            _context.SubSets.Remove(subSet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubSetExists(int id)
        {
            return _context.SubSets.Any(e => e.Id == id);
        }
    }
}
