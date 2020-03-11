using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Workouts
{
    public class Set : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Rutina")]
        public WorkoutRoutine WorkoutRoutine { get; set; }

        public virtual ICollection<SubSet> SubSets { get; set; }
    }
}
