using bFit.Web.Data.Entities.Profiles;
using System;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Financial
{
    public class Payment : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cliente")]
        public Membership Membership { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de inicio")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de fin")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de pago")]
        public PaymentType PaymentType { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Monto")]
        public float Amount { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Usuario")]
        public User User { get; set; }
    }
}
