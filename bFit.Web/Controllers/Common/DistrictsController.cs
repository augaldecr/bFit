using bFit.Web.Data;
using bFit.Web.Data.Entities.Common;
using bFit.Web.Helpers;
using bFit.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Controllers.Common
{
    [Authorize(Roles = "Admin")]
    public class DistrictsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public DistrictsController(ApplicationDbContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Districts
                .Include(d => d.County)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts
                .Include(d => d.County)
                    .ThenInclude(c => c.State)
                        .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        public async Task<IActionResult> Create()
        {
            var createDistrictVwm = new CreateDistrictViewModel
            {
                Countries = await _combosHelper.GetComboCountriesAsync(),
            };
            return View(createDistrictVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDistrictViewModel model)
        {
            if (ModelState.IsValid)
            {
                var district = await _converterHelper.ToDistrictAsync(model);

                _context.Add(district);
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

            District district = await _context.Districts.FindAsync(id);

            if (district == null)
            {
                return NotFound();
            }

            var editDistrictVwm = await _converterHelper.ToEditDistrictViewModelAsync(district);

            return View(editDistrictVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDistrictViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var district = await _converterHelper.ToDistrictAsync(model);
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(model.Id))
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

        // GET: Districts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            District district = await _context.Districts.FindAsync(id);
            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> GetDistrictsAsync(int id)
        {
            var districts = await _combosHelper.GetComboDistrictsAsync(id);

            return Json(districts);
        }

        private bool DistrictExists(int id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }
    }
}
