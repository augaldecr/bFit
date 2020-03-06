using bFit.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

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
    }
}
