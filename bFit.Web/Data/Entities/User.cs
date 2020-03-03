using bFit.Web.Data.Entities.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities
{
    public class User : IdentityUser
    {
        //Cédula
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cédula")]
        public string SocialSecurity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Primer apellido")]
        public string LastName1 { get; set; }

        [Display(Name = "Segundo apellido")]
        public string LastName2 { get; set; }

        //Ciudad, pueblo, villa, etc..
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Localidad")]
        public Town Town { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }
    }
}