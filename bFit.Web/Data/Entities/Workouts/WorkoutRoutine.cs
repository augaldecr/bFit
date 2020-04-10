using bFit.Web.Data.Entities.Profiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Workouts
{
    public class WorkoutRoutine : WorkoutTemplate
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime Begins { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de fin")]
        [DataType(DataType.Date)]
        public DateTime Ends { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cliente")]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Entrenador")]
        public Trainer Trainer { get; set; }

        public virtual ICollection<Set> Sets { get; set; }
    }
}