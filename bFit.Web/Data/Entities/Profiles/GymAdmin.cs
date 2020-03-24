using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Profiles
{
    public class GymAdmin : IFranchiseEmployee
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Usuario")]
        public User User { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Gimnasio")]
        public LocalGym LocalGym { get; set; }

        [HiddenInput(DisplayValue = false)]
        public virtual Franchise Franchise => LocalGym.Franchise;
    }
}