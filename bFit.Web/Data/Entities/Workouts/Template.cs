using bFit.Web.Data.Entities.Profiles;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Workouts
{
    public class Template : WorkoutTemplate
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public string Name { get; set; }

        public virtual ICollection<SetTemplate> SetTemplates { get; set; }

        [Required]
        [Display(Name = "Franquicia")]
        public Franchise Franchise { get; set; }

        [Display(Name = "Creador")]
        public User Creator { get; set; }
    }
}