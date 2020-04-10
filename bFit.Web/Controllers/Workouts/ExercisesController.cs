using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Data;
using bFit.Web.Helpers;
using bFit.Web.Models;

namespace bFit.Web.Controllers.Workouts
{
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ExercisesController(ApplicationDbContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            int? franchise = await GetFranchise();

            ICollection<Exercise> exercises = new List<Exercise>();

            if (franchise == 0 || franchise == null)
            {
                exercises = await _context.Exercises
                    .Include(e => e.ExerciseType)
                    .ToListAsync();
            } else
            {
                exercises = await _context.Exercises
                    .Include(e => e.ExerciseType)
                    .Where(e => e.Franchise.Id == franchise)
                    .ToListAsync();
            }

            return View(exercises);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.ExerciseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var exerciseVwm = new ExerciseViewModel
            {
                 ExerciseTypes = await _combosHelper.GetComboExerciseTypesAsync(),
            };
            return View(exerciseVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExerciseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exercise = await _converterHelper.ToExerciseAsync(model);

                await _context.AddAsync(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            var exerciseVwm = await _converterHelper.ToEditExerciseViewModel(exercise);

            return View(exerciseVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditExerciseViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var exercise = await _converterHelper.ToExerciseAsync(model);

                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(model.Id))
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

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }

        private async Task<int?> GetFranchise()
        {
            return await _userHelper.GetFranchise(User.Identity.Name);
        }
    }
}
