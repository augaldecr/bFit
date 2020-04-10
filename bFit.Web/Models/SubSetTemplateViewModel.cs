using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class SubSetTemplateViewModel
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int SetTemplateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cantidad de repeticiones")]
        [Range(1, 60, ErrorMessage = "El número de repeticiones debe ser menor a 60")]
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
        [Display(Name = "Ejercicio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un ejercicio")]
        public int ExerciseId { get; set; }

        public IEnumerable<SelectListItem> Exercises { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de ejercicio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de ejecución")]
        public int SubSetTypeId { get; set; }

        public IEnumerable<SelectListItem> SubSetTypes { get; set; }
    }
}
