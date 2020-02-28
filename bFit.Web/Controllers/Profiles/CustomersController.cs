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
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IEmployeeHelper _employeeHelper;

        public CustomersController(ApplicationDbContext context, 
            IUserHelper userHelper,
            IEmployeeHelper employeeHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _employeeHelper = employeeHelper;
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
                var gymAdmin = _context.GymAdmins.FirstOrDefaultAsync(
                    g => g.User.Email == "alonsougaldecr@gmail.com");
            }

            if (emp.GetType() == typeof(Trainer))
            {
                var trainer = _context.Trainers.FirstOrDefaultAsync(
                    t => t.User.Email == "alonsougaldecr@gmail.com");
            }

            return View( await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
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
