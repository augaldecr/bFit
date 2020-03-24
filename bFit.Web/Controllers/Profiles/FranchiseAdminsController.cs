using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Helpers;
using bFit.Web.Models;
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
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public FranchiseAdminsController(ApplicationDbContext context,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            ICombosHelper combosHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            var franchiseAdmins = await _context.FranchiseAdmins
                .Include(a => a.Franchise)
                .Include(a => a.User)
                    .ThenInclude(u => u.Town)
                .ToListAsync();
            return View(franchiseAdmins);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FranchiseAdmin franchiseAdmin = await _context.FranchiseAdmins
                .Include(f => f.Franchise)
                .Include(f => f.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (franchiseAdmin == null)
            {
                return NotFound();
            }

            return View(franchiseAdmin);
        }


        public IActionResult Create()
        {
            var adminFranchiseVwm = new CreateFranchiseAdminViewModel
            {
                Towns = _combosHelper.GetComboTowns(),
                Franchises = _combosHelper.GetComboFranchises(),
            };
            return View(adminFranchiseVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFranchiseAdminViewModel franchiseAdmin)
        {
            if (ModelState.IsValid)
            {
                var custom = await _userHelper.GetUserByEmailAsync(franchiseAdmin.Email);

                if (custom == null)
                {
                    var FranAdmin = await _converterHelper.ToFranchiseAdminAsync(franchiseAdmin);

                    await _userHelper.AddUserAsync(FranAdmin.User, "123456");
                    await _userHelper.AddUserToRoleAsync(FranAdmin.User, "FranchiseAdmin");

                    _context.FranchiseAdmins.Add(FranAdmin);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con ése correo electrónico");
                    return RedirectToAction($"{nameof(Create)}");
                }
            }
            return View(franchiseAdmin);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var franchiseAdmin = await _context.FranchiseAdmins
                .Include(a => a.User)
                    .ThenInclude(u => u.Town)
                .Include(a => a.Franchise)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (franchiseAdmin == null)
            {
                return NotFound();
            }

            var adminVwm = _converterHelper.ToFranchiseAdminViewModel(franchiseAdmin);

            return View(adminVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FranchiseAdminViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var franchiseAdmin = await _converterHelper.ToFranchiseAdminAsync(model);
                    _context.Update(franchiseAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FranchiseAdminExists(model.Id))
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
            return View(model);
        }

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
