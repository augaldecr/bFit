using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Controllers.Profiles
{
    [Authorize(Roles = "Admin")]
    public class GymAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymAdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GymAdmins
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymAdmins.ToListAsync());
        }

        // GET: GymAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GymAdmin gymAdmin = await _context.GymAdmins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymAdmin == null)
            {
                return NotFound();
            }

            return View(gymAdmin);
        }

        // GET: GymAdmins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymAdmins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] GymAdmin gymAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymAdmin);
        }

        // GET: GymAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GymAdmin gymAdmin = await _context.GymAdmins.FindAsync(id);
            if (gymAdmin == null)
            {
                return NotFound();
            }
            return View(gymAdmin);
        }

        // POST: GymAdmins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] GymAdmin gymAdmin)
        {
            if (id != gymAdmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymAdminExists(gymAdmin.Id))
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
            return View(gymAdmin);
        }

        // GET: GymAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GymAdmin gymAdmin = await _context.GymAdmins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymAdmin == null)
            {
                return NotFound();
            }

            return View(gymAdmin);
        }

        // POST: GymAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GymAdmin gymAdmin = await _context.GymAdmins.FindAsync(id);
            _context.GymAdmins.Remove(gymAdmin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymAdminExists(int id)
        {
            return _context.GymAdmins.Any(e => e.Id == id);
        }
    }
}
