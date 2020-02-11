using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Common
{
    //Distrito
    public class District : BasicEntity, IEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cantón")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public County County { get; set; }

        [Display(Name = "Ciudades")]
        public virtual ICollection<Town> Towns { get; set; }
    }
}
