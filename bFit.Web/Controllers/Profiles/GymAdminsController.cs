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
    [Authorize(Roles = "Admin, FranchiseAdmin")]
    public class GymAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public GymAdminsController(ApplicationDbContext context,
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
            if (User.IsInRole("Admin"))
            {
                return View(await _context.GymAdmins
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Franchise)
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Town)
                    .Include(g => g.User)
                        .ThenInclude(u => u.Town)
                    .ToListAsync());
            }
            else
            {
                var franchiseAdmin = await _context.FranchiseAdmins
                    .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);
                var franchiseId = franchiseAdmin.Franchise.Id;

                var gymAdmins = await _context.GymAdmins
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Franchise)
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Town)
                    .Include(g => g.User)
                        .ThenInclude(u => u.Town)
                    .Where(g => g.Franchise.Id == franchiseId)
                    .ToListAsync();
                return View(gymAdmins);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GymAdmin gymAdmin = await _context.GymAdmins
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Franchise)
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Town)
                .Include(f => f.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gymAdmin == null)
            {
                return NotFound();
            }

            return View(gymAdmin);
        }

        public async Task<IActionResult> Create()
        {
            CreateGymAdminViewModel createGymAdminView = new CreateGymAdminViewModel();

            if (User.IsInRole("Admin"))
            {
                createGymAdminView.Towns = _combosHelper.GetComboTowns();
                createGymAdminView.Gyms = await _combosHelper.GetComboGymsAsync(null);
            }
            else
            {
                var franchiseAdmin = await _context.FranchiseAdmins
                    .Include(f => f.Franchise)
                    .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);

                createGymAdminView.Towns = _combosHelper.GetComboTowns();
                createGymAdminView.Gyms =
                    await _combosHelper.GetComboGymsAsync(franchiseAdmin.Franchise.Id);
            }
            return View(createGymAdminView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGymAdminViewModel gymAdminModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(gymAdminModel.Email);

                if (user == null)
                {
                    var gymAdmin = await _converterHelper.ToGymAdminAsync(gymAdminModel);

                    await _userHelper.AddUserAsync(gymAdmin.User, "123456");
                    await _userHelper.AddUserToRoleAsync(gymAdmin.User, "GymAdmin");

                    _context.GymAdmins.Add(gymAdmin);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con ése correo electrónico");
                    return RedirectToAction($"{nameof(Create)}");
                }
            }
            return View(gymAdminModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymAdmin = await _context.GymAdmins
                .Include(g => g.LocalGym)
                    .ThenInclude(l => l.Franchise)
                .Include(g => g.LocalGym)
                    .ThenInclude(l => l.Town)
                .Include(g => g.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gymAdmin == null)
            {
                return NotFound();
            }

            GymAdminViewModel gymAdminModel = new GymAdminViewModel();

            if (User.IsInRole("Admin"))
            {
                gymAdminModel = await _converterHelper.ToGymAdminViewModelAsync(gymAdmin, null);
            }
            else
            {
                var franchiseAdmin =
                    await _context.FranchiseAdmins
                        .Include(f => f.Franchise)
                        .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);

                gymAdminModel =
                    await _converterHelper.ToGymAdminViewModelAsync(gymAdmin, franchiseAdmin.Franchise.Id);
            }

            return View(gymAdminModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GymAdminViewModel gymAdminModel)
        {
            if (id != gymAdminModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var gymAdmin = await _converterHelper.ToGymAdminAsync(gymAdminModel);
                    _context.Update(gymAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymAdminExists(gymAdminModel.Id))
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
            return View(gymAdminModel);
        }

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
