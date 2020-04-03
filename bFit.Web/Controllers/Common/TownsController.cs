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
    public class TownsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public TownsController(ApplicationDbContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Towns
                .Include(t => t.District)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Town town = await _context.Towns
                .Include(t => t.District)
                    .ThenInclude(d => d.County)
                        .ThenInclude(d => d.State)
                            .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (town == null)
            {
                return NotFound();
            }

            return View(town);
        }

        public async Task<IActionResult> Create()
        {
            var createTownsViewModel = new CreateTownViewModel
            {
                Countries = await _combosHelper.GetComboCountriesAsync(),
            };
            return View(createTownsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTownViewModel model)
        {
            if (ModelState.IsValid)
            {
                var town = await _converterHelper.ToTownAsync(model);
                await _context.AddAsync(town);
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

            Town town = await _context.Towns.FindAsync(id);
            if (town == null)
            {
                return NotFound();
            }

            var editTownVwm = await _converterHelper.ToEditTownViewModelAsync(town);

            return View(editTownVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTownViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var town = await _converterHelper.ToTownAsync(model);
                    _context.Update(town);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TownExists(model.Id))
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

        // GET: Towns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Town town = await _context.Towns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (town == null)
            {
                return NotFound();
            }

            return View(town);
        }

        // POST: Towns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Town town = await _context.Towns.FindAsync(id);
            _context.Towns.Remove(town);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> GetTownsAsync(int id)
        {
            var towns = await _combosHelper.GetComboTownsAsync(id);

            return Json(towns);
        }

        private bool TownExists(int id)
        {
            return _context.Towns.Any(e => e.Id == id);
        }
    }
}
