using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class CreateGymAdminViewModel : GymAdminViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
}