using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Common
{
    public class State : BasicEntity, IEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "País")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public Country Country { get; set; }

        [Display(Name = "Provincias")]
        public virtual ICollection<County> Counties { get; set; }
    }
}