using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Workouts
{
    public class SubSet : BasicEntity
    { 
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Ejercicio")]
        public Exercise Exercise { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cantidad de repeticiones")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tiempo en positivo")]
        [DataType(DataType.Duration)]
        public int PositiveTime { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tiempo en negativo")]
        [DataType(DataType.Duration)]
        public int NegativeTime { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Rutina de entrenamiento")]
        public WorkoutRoutine WorkoutRoutine { get; set; }
    }
}
