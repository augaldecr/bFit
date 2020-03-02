using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Helpers;
using bFit.Web.Models;
using bFit.WEB.Data.Entities.Profiles;
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

        public CustomersController(ApplicationDbContext context, 
            IUserHelper userHelper,
            IEmployeeHelper employeeHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _employeeHelper = employeeHelper;
            _converterHelper = converterHelper;
        }

        // GET: Customers  alonsougaldecr@gmail.com
        public async Task<IActionResult> Index()
        {
            var emp = _employeeHelper.EmployeeAsync("alonsougaldecr@gmail.com");

            IList<Customer> customers = new List<Customer>();

            if (emp.GetType() == typeof(FranchiseAdmin))
            {
                var franAdmin = await _context.FranchiseAdmins.FirstOrDefaultAsync(
                    f => f.User.Email == "alonsougaldecr@gmail.com");
                var franchise = await _context.Franchises.FirstOrDefaultAsync(
                    f => f == franAdmin.Franchise);
                customers = await _context.Customers.Where(
                    c => c.Gym.Franchise == franchise).ToListAsync();
            }

            if (emp.GetType() == typeof(GymAdmin))
            {
                var gymAdmin = await _context.GymAdmins.FirstOrDefaultAsync(
                    g => g.User.Email == "alonsougaldecr@gmail.com");
                var franchise = await _context.Franchises.FirstOrDefaultAsync(
                    f => f == gymAdmin.Franchise);
                customers = await _context.Customers.Where(
                    c => c.Gym.Franchise == franchise).ToListAsync();
            }

            if (emp.GetType() == typeof(Trainer))
            {
                var trainer = _context.Trainers
                    .Include(t => t.Franchise)
                    .FirstOrDefault();
                var franchise = _context.Franchises.FirstOrDefault(
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

            var customVWMs = new List<CustomerViewModel>();

            foreach (var customer in customers)
            {
                customVWMs.Add(_converterHelper.ToCustomerViewModel(customer));
            }

            return View(customVWMs);
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerVWM = _converterHelper.ToCustomerViewModel(customer);

            return View(customerVWM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
    }
}
