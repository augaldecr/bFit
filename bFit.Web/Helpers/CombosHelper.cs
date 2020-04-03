using bFit.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly ApplicationDbContext _context;

        public CombosHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboGoalsAsync()
        {
            List<SelectListItem> list = await _context.Goals.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una meta]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboExercisesAsync()
        {
            List<SelectListItem> list = await _context.Exercises.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un ejercicio]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboSubSetTypesAsync()
        {
            List<SelectListItem> list = await _context.SubSetTypes.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de ejercicio]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboGendersAsync()
        {
            List<SelectListItem> list = await _context.Genders.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un género]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboGymsAsync(int? id)
        {
            List<SelectListItem> list;

            if (id == null)
            {
                var gyms = await _context.Gyms
                    .Include(g => g.Franchise)
                    .Include(g => g.Town)
                    .ToListAsync();

                list = new List<SelectListItem>();

                foreach (var gym in gyms)
                {
                    list.Add(new SelectListItem
                    {
                        Text = $"{gym.Name}",
                        Value = $"{gym.Id}",
                    });
                }
                list = list.OrderBy(g => g.Text).ToList();
            }
            else
            {
                var gyms = _context.Gyms
                    .Include(g => g.Franchise)
                    .Include(g => g.Town)
                    .Where(g => g.Franchise.Id == id);
                /*list = gyms.Select(g =>  new SelectListItem
                {
                    Text = $"{g.Name}",
                    Value = $"{g.Id}"
                })
              .OrderBy(g => g.Text)
              .ToList(); */

                list = new List<SelectListItem>();

                foreach (var gym in gyms)
                {
                    list.Add(new SelectListItem
                    {
                        Text = $"{gym.Name}",
                        Value = $"{gym.Id}",
                    });
                }
                list = list.OrderBy(g => g.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un gimnasio]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboTrainersAsync(int? franchiseId)
        {
            List<SelectListItem> list;

            if (franchiseId == null)
            {
                var trainers = await _context.Trainers
                    .Include(t => t.User)
                    .ToListAsync();

                list = new List<SelectListItem>();

                foreach (var trainer in trainers)
                {
                    list.Add(new SelectListItem
                    {
                        Text = $"{trainer.User.FullName}",
                        Value = $"{trainer.Id}",
                    });
                }
                list = list.OrderBy(g => g.Text).ToList();
            }
            else
            {
                var trainers = _context.Trainers
                    .Include(t => t.User)
                    .Where(t => t.Franchise.Id == franchiseId);
                /*list = gyms.Select(g =>  new SelectListItem
                {
                    Text = $"{g.Name}",
                    Value = $"{g.Id}"
                })
              .OrderBy(g => g.Text)
              .ToList(); */

                list = new List<SelectListItem>();

                foreach (var trainer in trainers)
                {
                    list.Add(new SelectListItem
                    {
                        Text = $"{trainer.User.FullName}",
                        Value = $"{trainer.Id}",
                    });
                }
                list = list.OrderBy(g => g.Text).ToList();
            }
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboFranchisesAsync()
        {
            List<SelectListItem> list = await _context.Franchises.Select(g => new SelectListItem
            {
                Text = g.TradeName,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una franquicia]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
        {
            List<SelectListItem> list = await _context.Countries.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un país]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int id)
        {
            List<SelectListItem> list = await _context.States
                .Include(s => s.Country)
                .Where(s => s.Country.Id == id)
                .Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = $"{g.Id}"
                })
          .OrderBy(g => g.Text)
          .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una provincia]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountiesAsync(int id)
        {
            List<SelectListItem> list = await _context.Counties
                .Include(c => c.State)
                .Where(s => s.State.Id == id)
                .Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = $"{g.Id}"
                })
          .OrderBy(g => g.Text)
          .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un cantón]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboDistrictsAsync(int id)
        {
            List<SelectListItem> list = await _context.Districts
                .Include(s => s.County)
                .Where(s => s.County.Id == id)
                .Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = $"{g.Id}"
                })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un distrito]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboTownsAsync(int districtId)
        {
            List<SelectListItem> list = await _context.Towns
                .Include(t => t.District)
                .Where(t => t.District.Id == districtId)
                .Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = $"{g.Id}"
                })
                .OrderBy(g => g.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una localidad]",
                Value = "0"
            });

            return list;
        }
    }
}
