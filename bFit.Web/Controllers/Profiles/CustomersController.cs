﻿using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Helpers;
using bFit.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Create()
        {
            var customerVwm = new CustomerViewModel();
            customerVwm.Genders = _combosHelper.GetComboGenders();
            customerVwm.Towns = _combosHelper.GetComboTowns();

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
                    var customer = _converterHelper.ToCustomer(model);
                    _context.Add(model);
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

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Birthday")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
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

        // POST: Customers/Delete/5
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

            var workout = await _context.WorkoutRoutines
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

            if (workout == null)
            {
                return NotFound();
            }

            var workoutVwm = _converterHelper.ToEditWorkoutViewModel(workout);

            return View(workoutVwm);
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

            var subSetVwm = _converterHelper.ToEditSubSetViewModel(subSet);

            return View(subSetVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubSet(int id, EditSubSetViewModel editSubSetVwm)
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

                return RedirectToAction("EditWorkout",new { @id = editSubSetVwm.WorkoutId });
            }
            return View(subSet);
        }

    }
}
