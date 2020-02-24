using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Workouts
{
    public class SubSet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Ejercicio")]
        public Exercise Exercise { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cantidad de repeticiones")]
        public int Quantity { get; set; }

        [Display(Name = "Tiempo en positivo")]
        [DataType(DataType.Duration)]
        public int PositiveTime { get; set; }

        [Display(Name = "Tiempo en negativo")]
        [DataType(DataType.Duration)]
        public int NegativeTime { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Serie")]
        public Set Set { get; set; }
    }
}
