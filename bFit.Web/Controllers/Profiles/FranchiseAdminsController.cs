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
    public class FranchiseAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FranchiseAdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FranchiseAdmins
        public async Task<IActionResult> Index()
        {
            return View(await _context.FranchiseAdmins.ToListAsync());
        }

        // GET: FranchiseAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FranchiseAdmin franchiseAdmin = await _context.FranchiseAdmins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (franchiseAdmin == null)
            {
                return NotFound();
            }

            return View(franchiseAdmin);
        }

        // GET: FranchiseAdmins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FranchiseAdmins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] FranchiseAdmin franchiseAdmin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(franchiseAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(franchiseAdmin);
        }

        // GET: FranchiseAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FranchiseAdmin franchiseAdmin = await _context.FranchiseAdmins.FindAsync(id);
            if (franchiseAdmin == null)
            {
                return NotFound();
            }
            return View(franchiseAdmin);
        }

        // POST: FranchiseAdmins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] FranchiseAdmin franchiseAdmin)
        {
            if (id != franchiseAdmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(franchiseAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FranchiseAdminExists(franchiseAdmin.Id))
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
            return View(franchiseAdmin);
        }

        // GET: FranchiseAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FranchiseAdmin franchiseAdmin = await _context.FranchiseAdmins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (franchiseAdmin == null)
            {
                return NotFound();
            }

            return View(franchiseAdmin);
        }

        // POST: FranchiseAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            FranchiseAdmin franchiseAdmin = await _context.FranchiseAdmins.FindAsync(id);
            _context.FranchiseAdmins.Remove(franchiseAdmin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FranchiseAdminExists(int id)
        {
            return _context.FranchiseAdmins.Any(e => e.Id == id);
        }
    }
}
