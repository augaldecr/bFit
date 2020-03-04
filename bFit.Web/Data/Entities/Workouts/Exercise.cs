using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Workouts
{
    public class Exercise : BasicEntity
    {
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Display(Name ="Movimiento")]
        [DataType(DataType.Upload)]
        public byte[] Image { get; set; }

        [Display(Name ="Ilustración (vídeo)")]
        [DataType(DataType.Html)]
        public string VideoUrl { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de ejercicio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public ExerciseType ExerciseType { get; set; }

        public virtual ICollection<SubSet> SubSets { get; set; }
    }
}