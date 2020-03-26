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
    public class LocalGymsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public LocalGymsController(ApplicationDbContext context,
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
            return View(await _context.Gyms
                .Include(g => g.Franchise)
                .Include(g => g.Town)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LocalGym localGym = await _context.Gyms
                .Include(g => g.Franchise)
                .Include(g => g.Town)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (localGym == null)
            {
                return NotFound();
            }

            return View(localGym);
        }

        public IActionResult Create()
        {
            CreateGymViewModel createGymVwm = new CreateGymViewModel
            {
                Towns = _combosHelper.GetComboTowns(),
                Franchises = _combosHelper.GetComboFranchises(),
            };
            return View(createGymVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGymViewModel createGymView)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("FranchiseAdmin"))
                {
                    var fAdmin = await _context.FranchiseAdmins
                        .Include(f => f.User)
                        .FirstOrDefaultAsync(
                        f => f.User.Email == User.Identity.Name);
                    createGymView.FranchiseId = fAdmin.Franchise.Id;
                }

                var gym = await _converterHelper.ToLocalGym(createGymView);

                await _context.AddAsync(gym);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createGymView);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LocalGym localGym = await _context.Gyms
                .Include(g => g.Franchise)
                .Include(g => g.Town)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (localGym == null)
            {
                return NotFound();
            }

            var editGymVwm = _converterHelper.ToEditGymViewModel(localGym);

            return View(editGymVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditGymViewModel editGymView)
        {
            if (id != editGymView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var gym = await _converterHelper.ToLocalGymAsync(editGymView);

                    _context.Update(gym);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalGymExists(editGymView.Id))
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
            return View(editGymView
                );
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LocalGym localGym = await _context.Gyms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (localGym == null)
            {
                return NotFound();
            }

            return View(localGym);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            LocalGym localGym = await _context.Gyms.FindAsync(id);
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