using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class EditSubSetViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cantidad de repeticiones")]
        public int Quantity { get; set; }

        [Display(Name = "Tiempo en positivo")]
        [DataType(DataType.Duration)]
        public int PositiveTime { get; set; }

        [Display(Name = "Tiempo en negativo")]
        [DataType(DataType.Duration)]
        public int NegativeTime { get; set; }

        [Display(Name = "Observaciones")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Meta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una meta")]
        public int ExerciseId { get; set; }

        public IEnumerable<SelectListItem> Exercises { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Meta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una meta")]
        public int SubSetTypeId { get; set; }

        public IEnumerable<SelectListItem> SubSetTypes { get; set; }
    }
}
