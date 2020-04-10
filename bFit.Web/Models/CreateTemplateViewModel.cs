using bFit.Web.Data.Entities.Workouts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class CreateTemplateViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Meta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una meta")]
        public int GoalId { get; set; }

        public IEnumerable<SelectListItem> Goals { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Franquicia")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una franquicia")]
        public int? FranchiseId { get; set; }

        public IEnumerable<SelectListItem> Franchises { get; set; }

        public virtual ICollection<SetTemplate> SetTemplates { get; set; }
    }
}
