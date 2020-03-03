using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bFit.Web.Data.Entities.PersonalData;
using bFit.Web.Data;

namespace bFit.Web.Controllers.PersonalData
{
    public class DataTakesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataTakesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DataTakes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PersonalData.ToListAsync());
        }

        // GET: DataTakes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTake = await _context.PersonalData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataTake == null)
            {
                return NotFound();
            }

            return View(dataTake);
        }

        // GET: DataTakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataTakes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Height,Weight,MuscleEsquelethicalMassKG,FatMassKG,WaterMass,MetabolicAge,ChestBack,Waist,Abdomen,Hip,LeftArm,RightArm,LeftQuadriceps,RightQuadriceps,LeftCalf,RightCalf,LeftForearm,RightForearm,VisceralFatLevel,BasalMetabolicRate,RecommendedCaloricIntake")] DataTake dataTake)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataTake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataTake);
        }

        // GET: DataTakes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTake = await _context.PersonalData.FindAsync(id);
            if (dataTake == null)
            {
                return NotFound();
            }
            return View(dataTake);
        }

        // POST: DataTakes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Height,Weight,MuscleEsquelethicalMassKG,FatMassKG,WaterMass,MetabolicAge,ChestBack,Waist,Abdomen,Hip,LeftArm,RightArm,LeftQuadriceps,RightQuadriceps,LeftCalf,RightCalf,LeftForearm,RightForearm,VisceralFatLevel,BasalMetabolicRate,RecommendedCaloricIntake")] DataTake dataTake)
        {
            if (id != dataTake.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataTake);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataTakeExists(dataTake.Id))
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
            return View(dataTake);
        }

        // GET: DataTakes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTake = await _context.PersonalData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataTake == null)
            {
                return NotFound();
            }

            return View(dataTake);
        }

        // POST: DataTakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataTake = await _context.PersonalData.FindAsync(id);
            _context.PersonalData.Remove(dataTake);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataTakeExists(int id)
        {
            return _context.PersonalData.Any(e => e.Id == id);
        }
    }
}
