using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bFit.WEB.Data.Entities.Profiles;
using bFit.Web.Data;

namespace bFit.Web.Controllers.Profiles
{
    public class LocalGymsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocalGymsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LocalGyms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gyms.ToListAsync());
        }

        // GET: LocalGyms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localGym = await _context.Gyms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (localGym == null)
            {
                return NotFound();
            }

            return View(localGym);
        }

        // GET: LocalGyms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocalGyms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhoneNumber,Email,Address")] LocalGym localGym)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localGym);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localGym);
        }

        // GET: LocalGyms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localGym = await _context.Gyms.FindAsync(id);
            if (localGym == null)
            {
                return NotFound();
            }
            return View(localGym);
        }

        // POST: LocalGyms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhoneNumber,Email,Address")] LocalGym localGym)
        {
            if (id != localGym.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localGym);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalGymExists(localGym.Id))
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
            return View(localGym);
        }

        // GET: LocalGyms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localGym = await _context.Gyms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (localGym == null)
            {
                return NotFound();
            }

            return View(localGym);
        }

        // POST: LocalGyms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var localGym = await _context.Gyms.FindAsync(id);
            _context.Gyms.Remove(localGym);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalGymExists(int id)
        {
            return _context.Gyms.Any(e => e.Id == id);
        }
    }
}
