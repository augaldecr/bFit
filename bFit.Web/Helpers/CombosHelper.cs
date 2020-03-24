using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
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

        public IEnumerable<SelectListItem> GetComboGoals()
        {
            List<SelectListItem> list = _context.Goals.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una meta]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboExercises()
        {
            List<SelectListItem> list = _context.Exercises.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un ejercicio]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSubSetTypes()
        {
            List<SelectListItem> list = _context.SubSetTypes.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de ejercicio]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboGenders()
        {
            List<SelectListItem> list = _context.Genders.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToList();

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

            if (id==null)
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
            } else
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

        public IEnumerable<SelectListItem> GetComboTowns()
        {
            List<SelectListItem> list = _context.Towns.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una localidad]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboTrainers(int? franchiseId)
        {
            List<SelectListItem> list;

            if (franchiseId == null)
            {
                var trainers = _context.Trainers
                    .Include(t => t.User)
                    .ToList();

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
            } else
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

        public IEnumerable<SelectListItem> GetComboFranchises()
        {
            List<SelectListItem> list = _context.Franchises.Select(g => new SelectListItem
            {
                Text = g.TradeName,
                Value = $"{g.Id}"
            })
                .OrderBy(g => g.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una franquicia]",
                Value = "0"
            });

            return list;
        }
    }
}
