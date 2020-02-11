using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Common
{
    //Condado, provincia o cantón
    public class County : BasicEntity, IEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Provincia")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public State State { get; set; }

        [Display(Name = "Distritos")]
        public virtual ICollection<District> Districts { get; set; }
    }
}