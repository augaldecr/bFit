using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Workouts
{
    public class Set : BasicEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de serie")]
        public SetType SetType { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Rutina")]
        public WorkoutRoutine WorkoutRoutine { get; set; }

        public virtual ICollection<SubSet> SubSets { get; set; }
    }
}
