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
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public AdminsController(ApplicationDbContext context,
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
            var admins = await _context.Admins
                .Include(a => a.User)
                    .ThenInclude(u => u.Town)
                .ToListAsync();
            return View(admins);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin admin = await _context.Admins
                .Include(a => a.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        public IActionResult Create()
        {
            var adminVwm = new CreateAdminViewModel
            {
                Towns = _combosHelper.GetComboTowns()
            };
            return View(adminVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAdminViewModel adminVwm)
        {
            if (ModelState.IsValid)
            {
                var custom = await _userHelper.GetUserByEmailAsync(adminVwm.Email);

                if (custom == null)
                {
                    var admin = await _converterHelper.ToAdminAsync(adminVwm);

                    await _userHelper.AddUserAsync(admin.User, "123456");
                    await _userHelper.AddUserToRoleAsync(admin.User, "Admin");

                    _context.Admins.Add(admin);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con ése correo electrónico");
                    return RedirectToAction($"{nameof(Create)}");
                }
            }
            return View(adminVwm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            var adminVwm = _converterHelper.ToAdminViewModelAsync(admin);

            return View(adminVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.Id))
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
            return View(admin);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin admin = await _context.Admins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Admin admin = await _context.Admins.FindAsync(id);
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
    }
}
