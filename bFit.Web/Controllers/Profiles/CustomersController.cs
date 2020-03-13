using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Helpers;
using bFit.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: Customers  alonsougaldecr@gmail.com
        public async Task<IActionResult> Index()
        {
            var emp = _employeeHelper.EmployeeAsync("alonsougaldecr@gmail.com");
            var userType = _userHelper.TypeOfUser(emp);

            IList<Customer> customers = new List<Customer>();

            if (userType == UserType.FranchiseAdmin)
            {
                FranchiseAdmin franAdmin = await _context.FranchiseAdmins.FirstOrDefaultAsync(
                    f => f.User.Email == "alonsougaldecr@gmail.com");
                Franchise franchise = await _context.Franchises.FirstOrDefaultAsync(
                    f => f == franAdmin.Franchise);
                customers = await _context.Customers.Where(
                    c => c.Gym.Franchise == franchise).ToListAsync();
            }

            if (userType == UserType.GymAdmin)
            {
                GymAdmin gymAdmin = await _context.GymAdmins.FirstOrDefaultAsync(
                    g => g.User.Email == "alonsougaldecr@gmail.com");
                Franchise franchise = await _context.Franchises.FirstOrDefaultAsync(
                    f => f == gymAdmin.Franchise);
                customers = await _context.Customers.Where(
                    c => c.Gym.Franchise == franchise).ToListAsync();
            }

            if (userType == UserType.Trainer)
            {
                Trainer trainer = _context.Trainers
                    .Include(t => t.Franchise)
                    .FirstOrDefault();
                Franchise franchise = _context.Franchises.FirstOrDefault(
                    f => f.Id == trainer.Franchise.Id);
                customers = await _context.Customers
                    .Include(c => c.Gender)
                    .Include(c => c.User)
                    .ThenInclude(c => c.Town)
                    .Include(c => c.Gym)
                    .ThenInclude(g => g.Franchise)
                    .Where(
                    c => c.Gym.Franchise.Id == franchise.Id).ToListAsync();
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
                .Include(c => c.Gym)
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
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            int? franchise;
            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                franchise = null;
            }
            else if (await _userHelper.IsUserInRoleAsync(user, "FranchiseAdmin"))
            {
                var employee = await _context.FranchiseAdmins.FirstOrDefaultAsync(
                    a => a.User.Email == user.Email);
                franchise = employee.Franchise.Id;
            }
            else if (await _userHelper.IsUserInRoleAsync(user, "GymAdmin"))
            {
                var employee = await _context.GymAdmins.FirstOrDefaultAsync(
                    a => a.User.Email == user.Email);
                franchise = employee.Franchise.Id;
            }
            else if (await _userHelper.IsUserInRoleAsync(user, "Trainer"))
            {
                var employee = await _context.Trainers.FirstOrDefaultAsync(
                   a => a.User.Email == user.Email);
                franchise = employee.Franchise.Id;
            }
            else
            {
                franchise = 0;
            }

            var customerVwm = new CustomerViewModel();
            customerVwm.Genders = _combosHelper.GetComboGenders();
            customerVwm.Towns = _combosHelper.GetComboTowns();
            customerVwm.Gyms = _combosHelper.GetComboGyms(franchise);

            return View(customerVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var custom = await _userHelper.GetUserByEmailAsync(model.Email);

                if (custom == null)
                {
                    var customer = await _converterHelper.ToCustomerAsync(model);

                    var trainer = await _context.Trainers.FirstOrDefaultAsync(
                        t => t.User.Email == User.Identity.Name);

                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario registrado con ése correo electrónico");
                    return RedirectToAction($"{nameof(Create)}");
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
                .Include(c => c.Gym)
                    .ThenInclude(g => g.Franchise)
                .Include(c => c.Gym)
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
            return View(await _converterHelper.ToCustomerViewModelAsync(customer));
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
                    _context.Update(await _converterHelper.ToCustomerAsync(customerVwm));
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
                return RedirectToAction(nameof(Index));
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
            Customer customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
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

            var workout = await getWorkoutComplete(id);

            if (workout == null)
            {
                return NotFound();
            }

            var editWorkoutVwm = _converterHelper.ToEditWorkoutViewModel(workout);

            return View(editWorkoutVwm);
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

            var subSetVwm = _converterHelper.ToSubSetViewModel(subSet);

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

        public async Task<IActionResult> CreateWorkout(int customerId)
        {
            if (customerId == null)
            {
                return NotFound();
            }

            if (_context.Customers.Any(c => c.Id == customerId))
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
                Franchise franchise;
                IEnumerable<SelectListItem> comboTrainers = new List<SelectListItem>();
                IFranchiseEmployee employee = null;

                if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
                {
                    franchise = null;
                    employee = await _context.Trainers.FirstOrDefaultAsync();
                    comboTrainers = _combosHelper.GetComboTrainers(null);
                }
                else if (await _userHelper.IsUserInRoleAsync(user, "FranchiseAdmin"))
                {
                    employee = await _context.FranchiseAdmins.FirstOrDefaultAsync(
                        a => a.User.Email == user.Email);
                    franchise = employee.Franchise;
                    comboTrainers = _combosHelper.GetComboTrainers(franchise.Id);
                }
                else if (await _userHelper.IsUserInRoleAsync(user, "GymAdmin"))
                {
                    employee = await _context.GymAdmins.FirstOrDefaultAsync(
                        a => a.User.Email == user.Email);
                    franchise = employee.Franchise;
                    comboTrainers = _combosHelper.GetComboTrainers(franchise.Id);
                }
                else if (await _userHelper.IsUserInRoleAsync(user, "Trainer"))
                {
                    employee = await _context.Trainers.FirstOrDefaultAsync(
                       a => a.User.Email == user.Email);
                    franchise = employee.Franchise;
                    comboTrainers = _combosHelper.GetComboTrainers(franchise.Id);
                }
                else
                {
                    franchise = null;
                }

                var workoutVwm = new WorkoutViewModel
                {
                    Begins = DateTime.Now,
                    Ends = DateTime.Now.AddMonths(1),
                    Customer = customer,
                    CustomerId = customerId,
                    TrainerId = employee.Id,
                    Trainers = comboTrainers,
                    Goals = _combosHelper.GetComboGoals(),
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

                    var editWorkout = _converterHelper.ToEditWorkoutViewModel(workoutRoutine);

                    return RedirectToAction("EditWorkout", new { @id = editWorkout.Id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No puede registrar una rutina de entrenamiento que ya existe");
                    return RedirectToAction("EditWorkout", new { @id = workoutView.Id });
                }
            }
            return View(workoutView);
        }

        public IActionResult CreateSet(int? workoutId)
        {
            SubSetViewModel subSetView;
            if (workoutId == null)
            {
                return NotFound();
            }

            subSetView = new SubSetViewModel
            {
                WorkoutId = Convert.ToInt32(workoutId),
                Exercises = _combosHelper.GetComboExercises(),
                SubSetTypes = _combosHelper.GetComboSubSetTypes(),
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

        public async Task<IActionResult> EditSet(int id)
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

            var setVwm = _converterHelper.ToSetViewModel(set);
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
                return RedirectToAction($"{nameof(EditSet)}", new {@id = setView.SetId});
            }
            return View(setView);
        }

        private async Task<WorkoutRoutine> getWorkoutComplete(int? id)
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
             .Include(w => w.Trainer)
             .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
