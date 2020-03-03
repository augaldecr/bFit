using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Common
{
    //Ciudad, pueblo, villa, etc..
    public class Town : BasicEntity, IEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Distrito")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public District District { get; set; }
    }
}