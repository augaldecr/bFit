using bFit.Web.Data;
using bFit.WEB.Data.Entities.Workouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Controllers.Workouts
{
    public class SetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sets.ToListAsync());
        }

        // GET: Sets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @set = await _context.Sets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@set == null)
            {
                return NotFound();
            }

            return View(@set);
        }

        // GET: Sets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Set @set)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@set);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@set);
        }

        // GET: Sets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @set = await _context.Sets.FindAsync(id);
            if (@set == null)
            {
                return NotFound();
            }
            return View(@set);
        }

        // POST: Sets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Set @set)
        {
            if (id != @set.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@set);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SetExists(@set.Id))
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
            return View(@set);
        }

        // GET: Sets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @set = await _context.Sets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@set == null)
            {
                return NotFound();
            }

            return View(@set);
        }

        // POST: Sets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @set = await _context.Sets.FindAsync(id);
            _context.Sets.Remove(@set);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SetExists(int id)
        {
            return _context.Sets.Any(e => e.Id == id);
        }
    }
}
