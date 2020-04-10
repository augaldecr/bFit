using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Workouts
{
    public abstract class WorkoutTemplate : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Meta")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public Goal Goal { get; set; }
    }
}
