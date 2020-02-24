using bFit.Web.Data;
using bFit.WEB.Data.Entities.Workouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Controllers.Workouts
{
    public class WorkoutRoutinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutRoutinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkoutRoutines
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkoutRoutines.ToListAsync());
        }

        // GET: WorkoutRoutines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutRoutine = await _context.WorkoutRoutines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workoutRoutine == null)
            {
                return NotFound();
            }

            return View(workoutRoutine);
        }

        // GET: WorkoutRoutines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkoutRoutines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Begins,Ends")] WorkoutRoutine workoutRoutine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workoutRoutine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workoutRoutine);
        }

        // GET: WorkoutRoutines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutRoutine = await _context.WorkoutRoutines.FindAsync(id);
            if (workoutRoutine == null)
            {
                return NotFound();
            }
            return View(workoutRoutine);
        }

        // POST: WorkoutRoutines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Begins,Ends")] WorkoutRoutine workoutRoutine)
        {
            if (id != workoutRoutine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workoutRoutine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutRoutineExists(workoutRoutine.Id))
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
            return View(workoutRoutine);
        }

        // GET: WorkoutRoutines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutRoutine = await _context.WorkoutRoutines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workoutRoutine == null)
            {
                return NotFound();
            }

            return View(workoutRoutine);
        }

        // POST: WorkoutRoutines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutRoutine = await _context.WorkoutRoutines.FindAsync(id);
            _context.WorkoutRoutines.Remove(workoutRoutine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutRoutineExists(int id)
        {
            return _context.WorkoutRoutines.Any(e => e.Id == id);
        }
    }
}
