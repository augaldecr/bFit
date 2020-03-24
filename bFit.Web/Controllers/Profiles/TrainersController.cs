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
    [Authorize(Roles = "Admin, FranchiseAdmin, GymAdmin")]
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public TrainersController(ApplicationDbContext context,
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
                return View(await _context.Trainers
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Franchise)
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Town)
                    .Include(g => g.User)
                        .ThenInclude(u => u.Town)
                    .ToListAsync());
            }
            else if (User.IsInRole("FranchiseAdmin"))
            {
                var franchiseAdmin = await _context.FranchiseAdmins
                    .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);
                var franchiseId = franchiseAdmin.Franchise.Id;

                var trainers = await _context.Trainers
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Franchise)
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Town)
                    .Include(g => g.User)
                        .ThenInclude(u => u.Town)
                    .Where(g => g.Franchise.Id == franchiseId)
                    .ToListAsync();
                return View(trainers);
            }
            else
            {
                var gymAdmin = await _context.GymAdmins
                    .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);
                var franchiseId = gymAdmin.Franchise.Id;

                var trainers = await _context.Trainers
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Franchise)
                    .Include(g => g.LocalGym)
                        .ThenInclude(l => l.Town)
                    .Include(g => g.User)
                        .ThenInclude(u => u.Town)
                    .Where(g => g.Franchise.Id == franchiseId)
                    .ToListAsync();
                return View(trainers);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trainer trainer = await _context.Trainers
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Franchise)
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Town)
                .Include(f => f.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        public async Task<IActionResult> Create()
        {
            CreateTrainerViewModel createTrainerView = new CreateTrainerViewModel();

            if (User.IsInRole("Admin"))
            {
                createTrainerView.Towns = _combosHelper.GetComboTowns();
                createTrainerView.Gyms = await _combosHelper.GetComboGymsAsync(null);
            }
            else if (User.IsInRole("FranchiseAdmin"))
            {
                var franchiseAdmin = await _context.FranchiseAdmins
                    .Include(f => f.Franchise)
                    .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);

                createTrainerView.Towns = _combosHelper.GetComboTowns();
                createTrainerView.Gyms =
                    await _combosHelper.GetComboGymsAsync(franchiseAdmin.Franchise.Id);
            }
            else
            {
                var gymAdmin = await _context.GymAdmins
                    .Include(f => f.LocalGym)
                    .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);

                createTrainerView.Towns = _combosHelper.GetComboTowns();
                //createTrainerView.Gyms =
                //    await _combosHelper.GetComboGymsAsync(gymAdmin.Franchise.Id);
                createTrainerView.GymId = gymAdmin.LocalGym.Id;
            }
            return View(createTrainerView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTrainerViewModel trainerModel)
        {
            if (ModelState.IsValid)
            {
                var trainer = await _converterHelper.ToTrainerAsync(trainerModel);

                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainerModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(g => g.LocalGym)
                    .ThenInclude(l => l.Franchise)
                .Include(g => g.LocalGym)
                    .ThenInclude(l => l.Town)
                .Include(g => g.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (trainer == null)
            {
                return NotFound();
            }

            TrainerViewModel trainerView = new TrainerViewModel();

            if (User.IsInRole("Admin"))
            {
                trainerView = await _converterHelper.ToTrainerViewModelAsync(trainer, null);
            }
            else if (User.IsInRole("FranchiseAdmin"))
            {
                var franchiseAdmin =
                    await _context.FranchiseAdmins
                        .Include(f => f.Franchise)
                        .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);

                trainerView =
                    await _converterHelper.ToTrainerViewModelAsync(trainer, franchiseAdmin.Franchise.Id);
            } else
            {
                var gymAdmin =
                    await _context.GymAdmins
                        .Include(f => f.LocalGym)
                            .ThenInclude(g => g.Franchise)
                        .FirstOrDefaultAsync(f => f.User.Email == User.Identity.Name);

                trainerView =
                    await _converterHelper.ToTrainerViewModelAsync(trainer, gymAdmin.Franchise.Id);
            }
            return View(trainerView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var trainer = await _converterHelper.ToTrainerAsync(model);
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(model.Id))
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

            Trainer trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Trainer trainer = await _context.Trainers.FindAsync(id);
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.Id == id);
        }
    }
}
