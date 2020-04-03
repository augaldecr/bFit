using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Models
{
    public class CreateDistrictViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Cantón")]
        public int CountyId { get; set; }

        public IEnumerable<SelectListItem> Counties { get; set; }

        [Display(Name = "País")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Provincia")]
        public int StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }
    }
}
