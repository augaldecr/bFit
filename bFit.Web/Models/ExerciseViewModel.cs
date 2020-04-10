using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class ExerciseViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Display(Name = "Movimiento")]
        [DataType(DataType.Upload)]
        public byte[] Image { get; set; }

        [Display(Name = "Ilustración (vídeo)")]
        [DataType(DataType.Html)]
        public string VideoUrl { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de ejercicio")]
        public int ExerciseTypeId { get; set; }

        public IEnumerable<SelectListItem> ExerciseTypes { get; set; }
    }
}
