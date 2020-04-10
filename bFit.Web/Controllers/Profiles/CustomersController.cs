using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Helpers;
using bFit.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Controllers.Profiles
{
    [Authorize(Roles = "Admin, FranchiseAdmin, GymAdmin, Trainer")]

    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IEmployeeHelper _employeeHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;

        public CustomersController(ApplicationDbContext context,
            IUserHelper userHelper,
            IEmployeeHelper employeeHelper,
            IConverterHelper converterHelper,
            ICombosHelper combosHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _employeeHelper = employeeHelper;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            IList<Customer> customers = new List<Customer>();
            var email = User.Identity.Name;
            var franchise = await GetFranchise();

            if (User.IsInRole("Admin"))
            {
                customers = await _context.Customers
                    .Include(c => c.User)
                        .ThenInclude(u => u.Town)
                    .Include(c => c.Gender)
                    .Include(c => c.Memberships)
                        .ThenInclude(m => m.Customer)
                    .Include(c => c.Memberships)
                        .ThenInclude(m => m.LocalGym)
                            .ThenInclude(g => g.Franchise)
                    .ToListAsync();
            }
            else
            {
                customers = await _context.Memberships
                    .Include(m => m.LocalGym)
                    .Where(m => m.LocalGym.Id == franchise && m.Active == true)
                    .Select(c => c.Customer)
                    .ToListAsync();
            }

            return View(customers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Gender)
                .Include(c => c.Somatotype)
                .Include(c => c.Memberships)
                    .ThenInclude(m => m.LocalGym)
                .Include(c => c.WorkOutRoutines)
                    .ThenInclude(w => w.Sets)
                        .ThenInclude(s => s.SubSets)
                            .ThenInclude(x => x.Exercise)
                                .ThenInclude(e => e.ExerciseType)
                .Include(c => c.WorkOutRoutines)
                    .ThenInclude(w => w.Trainer)
                .Include(c => c.WorkOutRoutines)
                    .ThenInclude(w => w.Goal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Create()
        {
            var customerVwm = new CreateCustomerViewModel
            {
                Genders = await _combosHelper.GetComboGendersAsync(),
                Countries = await _combosHelper.GetComboCountriesAsync(),
                Gyms = await _combosHelper.GetComboGymsAsync(await GetFranchise()),
                Somatotypes = await _combosHelper.GetComboSomatypesAsync()
            };

            return View(customerVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var custom = await _context.Customers
                    .FirstOrDefaultAsync(c => c.User.Email == model.Email);

                if (custom == null)
                {
                    var customer = await _converterHelper.ToCustomerAsync(model);

                    await _context.AddAsync(customer);

                    var membership = new Membership
                    {
                        Customer = customer,
                        LocalGym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == model.GymId),
                        Active = true,
                    };

                    await _context.AddAsync(membership);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var membership = _context.Memberships
                        .FirstOrDefaultAsync(m => m.Customer.User.Email == model.Email &&
                            m.LocalGym.Id == model.GymId);

                    if (membership == null)
                    {
                        var customer = await _converterHelper.ToCustomerAsync(model);
                        var membershipNew = new Membership
                        {
                            Customer = customer,
                            LocalGym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == model.GymId),
                            Active = true,
                        };

                        await _context.AddAsync(membership);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                            "Ya existe un usuario registrado con ése correo electrónico");
                        return RedirectToAction($"{nameof(Create)}");
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = await _context.Customers
                .Include(c => c.User)
                .Include(c => c.Gender)
                .Include(c => c.Somatotype)
                .Include(c => c.Memberships)
                    .ThenInclude(m => m.LocalGym)
                        .ThenInclude(g => g.Franchise)
                .Include(c => c.Memberships)
                    .ThenInclude(m => m.LocalGym)
                        .ThenInclude(g => g.Town)
                .Include(c => c.WorkOutRoutines)
                    .ThenInclude(w => w.Sets)
                        .ThenInclude(s => s.SubSets)
                            .ThenInclude(x => x.Exercise)
                                .ThenInclude(e => e.ExerciseType)
                .Include(c => c.WorkOutRoutines)
                    .ThenInclude(w => w.Trainer)
                .Include(c => c.WorkOutRoutines)
                    .ThenInclude(w => w.Goal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerVwm = await _converterHelper.ToCustomerViewModelAsync(customer);
            customerVwm.Gyms = await _combosHelper.GetComboGymsAsync(await GetFranchise());

            return View(customerVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customerVwm)
        {
            if (id != customerVwm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var customer = await _converterHelper.ToCustomerAsync(customerVwm);
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customerVwm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { @id = customerVwm.Id });
            }
            return View(customerVwm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? franchise = await GetFranchise();

            if (franchise != null)
            {
                var membership = await _context.Memberships
                    .Include(m => m.Customer)
                    .Include(m => m.LocalGym)
                        .ThenInclude(g => g.Memberships)
                    .FirstOrDefaultAsync(m => m.LocalGym.Franchise.Id == franchise &&
                                            m.Customer.Id == id);
                membership.Active = false;

                _context.Update(membership);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        public async Task<IActionResult> EditWorkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await GetWorkoutCompleteAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            var editWorkoutVwm = await _converterHelper.ToEditWorkoutViewModelAsync(workout);

            return View(editWorkoutVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWorkout(int? id, EditWorkoutViewModel editWorkoutView)
        {
            if (id != editWorkoutView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var workout = await _converterHelper.ToWorkoutAsync(editWorkoutView);

                _context.Update(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditWorkout), new { @id, editWorkoutView.Id });
            }
            return View(editWorkoutView);
        }

        public async Task<IActionResult> EditSubSet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSet = await _context.SubSets
                .Include(s => s.Exercise)
                    .ThenInclude(e => e.ExerciseType)
                .Include(s => s.SubSetType)
                .Include(s => s.Set)
                    .ThenInclude(t => t.WorkoutRoutine)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (subSet == null)
            {
                return NotFound();
            }

            var subSetVwm = await _converterHelper.ToSubSetViewModelAsync(subSet);

            return View(subSetVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubSet(int id, SubSetViewModel editSubSetVwm)
        {
            if (id != editSubSetVwm.Id)
            {
                return NotFound();
            }

            var subSet = await _converterHelper.ToSubSetAsync(editSubSetVwm);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(subSet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("EditWorkout", new { @id = editSubSetVwm.WorkoutId });
            }
            return View(subSet);
        }

        public async Task<IActionResult> CreateWorkout(int? customerId)
        {
            if (customerId == null)
            {
                return NotFound();
            }

            if (_context.Customers.Any(c => c.Id == customerId))
            {
                var workoutVwm = new WorkoutViewModel
                {
                    Begins = DateTime.Now,
                    Ends = DateTime.Now.AddMonths(1),
                    Customer = await _context.Customers.FindAsync(customerId),
                    CustomerId = (int)customerId,
                    Trainers = await _combosHelper.GetComboTrainersAsync(await GetFranchise()),
                    Goals = await _combosHelper.GetComboGoalsAsync(),
                    Sets = null,
                };

                return View(workoutVwm);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWorkout(WorkoutViewModel workoutView)
        {
            if (ModelState.IsValid)
            {
                var workout = await _context.WorkoutRoutines.FirstOrDefaultAsync(w => w.Id == workoutView.Id);

                if (workout == null)
                {
                    var workoutRoutine = await _converterHelper.ToWorkoutAsync(workoutView);

                    _context.Add(workoutRoutine);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("EditWorkout", new { @id = workoutRoutine.Id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No puede registrar una rutina de entrenamiento que ya existe");
                    return RedirectToAction("EditWorkout", new { @id = workoutView.Id });
                }
            }
            return View(workoutView);
        }

        public async Task<IActionResult> CreateSetAsync(int? workoutId)
        {
            SubSetViewModel subSetView;
            if (workoutId == null)
            {
                return NotFound();
            }

            subSetView = new SubSetViewModel
            {
                WorkoutId = Convert.ToInt32(workoutId),
                Exercises = await _combosHelper.GetComboExercisesAsync(),
                SubSetTypes = await _combosHelper.GetComboSubSetTypesAsync(),
            };

            return View(subSetView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSet(SubSetViewModel subSetView)
        {
            if (ModelState.IsValid)
            {
                var workout = await _context.WorkoutRoutines.FirstOrDefaultAsync(w => w.Id == subSetView.WorkoutId);

                if (workout != null)
                {
                    var set = new Set
                    {
                        WorkoutRoutine = workout,
                    };

                    await _context.AddAsync(set);
                    await _context.SaveChangesAsync();

                    var subSet = await _converterHelper.ToSubSetAsync(subSetView);
                    subSet.Set = set;

                    await _context.AddAsync(subSet);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EditSet", new { @id = set.Id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un circuito registrado con ése identificador");
                    return RedirectToAction($"{nameof(CreateWorkout)}");
                }
            }
            return View(subSetView);
        }

        public async Task<IActionResult> EditSet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var set = await _context.Sets
                .Include(s => s.SubSets)
                    .ThenInclude(ss => ss.Exercise)
                        .ThenInclude(e => e.ExerciseType)
                .Include(s => s.WorkoutRoutine)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (set == null)
            {
                return NotFound();
            }

            var setVwm = await _converterHelper.ToSetViewModelAsync(set);
            return View(setVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSet(SetViewModel setView)
        {
            if (ModelState.IsValid)
            {
                var subset = await _converterHelper.ToSubSetAsync(setView);
                subset.Id = 0;

                await _context.AddAsync(subset);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(EditSet)}", new { @id = setView.SetId });
            }
            return View(setView);
        }

        public async Task<IActionResult> DetailsWorkout(int id)
        {
            if (await _context.WorkoutRoutines.AnyAsync(w => w.Id == id))
            {
                var workout = await GetWorkoutCompleteAsync(id);
                return View(workout);
            }

            return NotFound();
        }

        public async Task<IActionResult> PrintWorkout(int id)
        {
            if (await _context.WorkoutRoutines.AnyAsync(w => w.Id == id))
            {
                var workout = await GetWorkoutCompleteAsync(id);
                return View(workout);
            }

            return NotFound();
        }

        private async Task<WorkoutRoutine> GetWorkoutCompleteAsync(int? id)
        {
            return await _context.WorkoutRoutines
             .Include(w => w.Goal)
             .Include(w => w.Sets)
                 .ThenInclude(s => s.SubSets)
                     .ThenInclude(x => x.SubSetType)
             .Include(w => w.Sets)
                 .ThenInclude(s => s.SubSets)
                     .ThenInclude(x => x.Exercise)
                         .ThenInclude(e => e.ExerciseType)
             .Include(w => w.Customer)
                .ThenInclude(c => c.User)
             .Include(w => w.Customer)
                .ThenInclude(c => c.Memberships)
                    .ThenInclude(m => m.LocalGym)
                        .ThenInclude(g => g.Town)
             .Include(w => w.Customer)
                .ThenInclude(c => c.Memberships)
                    .ThenInclude(m => m.LocalGym)
                        .ThenInclude(g => g.Franchise)
             .Include(w => w.Trainer)
                .ThenInclude(t => t.User)
             .FirstOrDefaultAsync(w => w.Id == id);
        }

        private async Task<int?> GetFranchise()
        {
            return await _userHelper.GetFranchise(User.Identity.Name);
        }

        private async Task<IList<Customer>> GetCustomers()
        {
            return await _context.Customers
                    .Include(c => c.Gender)
                    .Include(c => c.Memberships)
                        .ThenInclude(m => m.LocalGym)
                            .ThenInclude(g => g.Town)
                    .Include(c => c.Memberships)
                        .ThenInclude(m => m.LocalGym)
                            .ThenInclude(g => g.Franchise)
                    .Include(c => c.User)
                        .ThenInclude(u => u.Town)
                    .ToListAsync();
        }

        private async Task<bool> MembershipExist(int id)
        {
            var franchise = await GetFranchise();
            bool membership;
            if (franchise != null)
            {
                membership = await _context.Memberships.AnyAsync(m => m.Customer.Id == id &&
                   m.LocalGym.Franchise.Id == franchise);
            }
            else
            {
                membership = await _context.Memberships.AnyAsync(m => m.Customer.Id == id);
            }

            return membership;

        }
    }
}
