using bFit.Web.Data;
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
    public class CountiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public CountiesController(ApplicationDbContext context,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Counties
                .Include(c => c.State)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var county = await _context.Counties
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (county == null)
            {
                return NotFound();
            }

            return View(county);
        }

        public async Task<IActionResult> Create()
        {
            var createCountyVwm = new CreateCountyViewModel
            {
                Countries = await _combosHelper.GetComboCountriesAsync(),
            };
            return View(createCountyVwm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCountyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var county = await _converterHelper.ToCountyAsync(model);
                _context.Add(county);
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

            var county = await _context.Counties
                .Include(c => c.State)
                    .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (county == null)
            {
                return NotFound();
            }

            var countyVm = await _converterHelper.ToEditCountyViewModelAsync(county);
            return View(countyVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCountyViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var county = await _converterHelper.ToCountyAsync(model);

                    _context.Update(county);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountyExists(model.Id))
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

            var county = await _context.Counties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (county == null)
            {
                return NotFound();
            }

            return View(county);
        }

        // POST: Counties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var county = await _context.Counties.FindAsync(id);
            _context.Counties.Remove(county);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> GetCountiesAsync(int id)
        {
            var counties = await _combosHelper.GetComboCountiesAsync(id);

            return Json(counties);
        }

        private bool CountyExists(int id)
        {
            return _context.Counties.Any(e => e.Id == id);
        }
    }
}
