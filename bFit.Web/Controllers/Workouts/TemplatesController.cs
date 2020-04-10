using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Helpers;
using bFit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Controllers.Workouts
{
    public class TemplatesController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly ApplicationDbContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public TemplatesController(ApplicationDbContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> AssignToCustomerAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .Include(t => t.Franchise)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var userFranchise = await GetFranchise();

            if (await _userHelper.IsUserInRoleAsync(user, "Admin") ||
                template.Franchise.Id == userFranchise)
            {
                ICollection<Customer> customers = new List<Customer>();
                ICollection<Template> templates = new List<Template>();

                int? trainerId = null;

                if (await _userHelper.IsUserInRoleAsync(user, "Trainer"))
                {
                    var trainer = await _context.Trainers
                        .Include(t => t.User)
                        .FirstOrDefaultAsync(t => t.User.Email == User.Identity.Name);
                    trainerId = trainer.Id;
                }

                if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
                {
                    customers = await _context.Memberships
                        .Include(m => m.Customer)
                            .ThenInclude(c => c.User)
                        .Select(m => m.Customer)
                        .ToListAsync();
                }
                else
                {
                    if (await GetGym() != null)
                    {
                        int gymId = (int)await GetGym();
                        customers = await _context.Memberships
                            .Include(m => m.Customer)
                                .ThenInclude(c => c.User)
                            .Where(m => m.LocalGym.Id == gymId)
                            .Select(m => m.Customer)
                            .ToListAsync();
                    }
                    else
                    {
                        int franchiseId = Convert.ToInt32(await GetFranchise());
                        customers = await _context.Memberships
                            .Include(m => m.Customer)
                                .ThenInclude(c => c.User)
                            .Where(m => m.LocalGym.Franchise.Id == franchiseId)
                            .Select(m => m.Customer)
                            .ToListAsync();
                    }
                }

                AssignWorkoutToCustomerViewModel assignWorkoutVwm = new AssignWorkoutToCustomerViewModel
                {
                    Customers = _combosHelper.GetComboCustomers(customers),
                    Goals = await _combosHelper.GetComboGoalsAsync(),
                    TemplateId = Convert.ToInt32(id),
                    Trainers = await _combosHelper.GetComboTrainersAsync(trainerId),
                };

                return View(assignWorkoutVwm);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToCustomerAsync(int id, AssignWorkoutToCustomerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var template = await _context.Templates
                    .Include(t => t.Goal)
                    .Include(t => t.SetTemplates)
                        .ThenInclude(s => s.SubSetTemplates)
                            .ThenInclude(ss => ss.Exercise)
                                .ThenInclude(e => e.ExerciseType)
                    .Include(t => t.SetTemplates)
                        .ThenInclude(s => s.SubSetTemplates)
                            .ThenInclude(ss => ss.SubSetType)
                    .FirstOrDefaultAsync(t => t.Id == model.Id);

                var customer = await _context.Customers.FindAsync(model.CustomerId);

                var workout = await _converterHelper.ToWorkoutAsync(model);
                workout.Customer = customer;

                await _context.AddAsync(workout);
                await _context.SaveChangesAsync();

                foreach (SetTemplate item in template.SetTemplates)
                {
                    var set = await _converterHelper.ToSetAsync(item);
                    set.WorkoutRoutine = workout;

                    await _context.AddAsync(set);
                    await _context.SaveChangesAsync();


                    foreach (var ss in item.SubSetTemplates)
                    {
                        var subSet = await _converterHelper.ToSubSetAsync(ss);
                        subSet.Set = set;

                        await _context.AddAsync(set);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(
                    Profiles.CustomersController.Details),
                    new { @id = customer.Id });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var template = await _converterHelper.ToTemplateAsync(model);
                template.Creator = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (model.FranchiseId == null)
                {
                    template.Franchise = await _context.Franchises
                    .FindAsync(await GetFranchise());
                }
                template.Franchise = await _context.Franchises.FindAsync(model.FranchiseId);

                _context.Add(template);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { @id = template.Id });
            }
            return View(model);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            var templateVwm = new CreateTemplateViewModel
            {
                Goals = await _combosHelper.GetComboGoalsAsync(),
            };

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                templateVwm.Franchises = await _combosHelper.GetComboFranchisesAsync();
            }

            return View(templateVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSet(SubSetTemplateViewModel subSetView)
        {
            if (ModelState.IsValid)
            {
                var template = await _context.Templates
                    .FindAsync(subSetView.TemplateId);

                if (template != null)
                {
                    var setTemplate = new SetTemplate
                    {
                        Template = template,
                    };

                    await _context.AddAsync(setTemplate);
                    await _context.SaveChangesAsync();

                    var subSetTemplate = await _converterHelper.ToSubSetTemplateAsync(subSetView);
                    subSetTemplate.SetTemplate = setTemplate;

                    await _context.AddAsync(subSetTemplate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("EditSet", new { @id = setTemplate.Id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un circuito registrado con ése identificador");
                    return RedirectToAction($"{nameof(Create)}");
                }
            }
            return View(subSetView);
        }

        public async Task<IActionResult> CreateSetAsync(int? templateId)
        {
            if (templateId == null)
            {
                return NotFound();
            }

            var subSetView = new SubSetTemplateViewModel
            {
                TemplateId = Convert.ToInt32(templateId),
                Exercises = await _combosHelper.GetComboExercisesAsync(),
                SubSetTypes = await _combosHelper.GetComboSubSetTypesAsync(),
            };

            return View(subSetView);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            _context.Templates.Remove(template);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .Include(t => t.Goal)
                .Include(t => t.Creator)
                .Include(t => t.SetTemplates)
                    .ThenInclude(s => s.SubSetTemplates)
                        .ThenInclude(ss => ss.Exercise)
                .Include(t => t.SetTemplates)
                    .ThenInclude(s => s.SubSetTemplates)
                        .ThenInclude(ss => ss.SubSetType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .Include(t => t.Goal)
                .Include(t => t.Creator)
                .Include(t => t.Franchise)
                .Include(t => t.SetTemplates)
                    .ThenInclude(s => s.SubSetTemplates)
                        .ThenInclude(ss => ss.Exercise)
                .Include(t => t.SetTemplates)
                    .ThenInclude(s => s.SubSetTemplates)
                        .ThenInclude(ss => ss.SubSetType)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (template == null)
            {
                return NotFound();
            }

            var templateVwm = await _converterHelper.ToEditTemplateViewModelAsync(template);

            return View(templateVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTemplateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var template = await _converterHelper.ToTemplateAsync(model);

                    _context.Update(template);
                    await _context.SaveChangesAsync();
                }
                catch (Exception x)
                {
                    throw new Exception(x.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> EditSet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var set = await _context.SetTemplates
                .Include(s => s.SubSetTemplates)
                    .ThenInclude(ss => ss.Exercise)
                        .ThenInclude(e => e.ExerciseType)
                .Include(s => s.SubSetTemplates)
                    .ThenInclude(ss => ss.SubSetType)
                .Include(s => s.Template)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (set == null)
            {
                return NotFound();
            }

            var setVwm = await _converterHelper.ToSetTemplateViewModelAsync(set);
            return View(setVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSet(SetTemplateViewModel setView)
        {
            if (ModelState.IsValid)
            {
                var subset = await _converterHelper.ToSubSetTemplateAsync(setView);
                subset.Id = 0;

                await _context.AddAsync(subset);
                await _context.SaveChangesAsync();
                return RedirectToAction($"{nameof(EditSet)}", new { @id = setView.Id });
            }
            return View(setView);
        }

        public async Task<IActionResult> EditSubSet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSetTemplate = await _context.SubSetTemplates
                .Include(s => s.Exercise)
                    .ThenInclude(e => e.ExerciseType)
                .Include(s => s.SubSetType)
                .Include(s => s.SetTemplate)
                    .ThenInclude(t => t.Template)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (subSetTemplate == null)
            {
                return NotFound();
            }

            var subSetVwm = await _converterHelper.ToSubSetTemplateViewModel(subSetTemplate);

            return View(subSetVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubSet(int id, SubSetTemplateViewModel editSubSetVwm)
        {
            if (id != editSubSetVwm.Id)
            {
                return NotFound();
            }

            var subSet = await _converterHelper.ToSubSetTemplateAsync(editSubSetVwm);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplateExists(subSet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Edit), new { @id = editSubSetVwm.TemplateId });
            }
            return View(subSet);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Templates
            .Include(t => t.Goal)
            .Include(t => t.Creator)
            .ToListAsync());
        }
        private async Task<int?> GetFranchise()
        {
            var email = User.Identity.Name;
            return await _userHelper.GetFranchise(email);
        }

        private async Task<int?> GetGym()
        {
            var email = User.Identity.Name;
            return await _userHelper.GetGym(email);
        }

        private bool TemplateExists(int id)
        {
            return _context.Templates.Any(e => e.Id == id);
        }
    }
}
