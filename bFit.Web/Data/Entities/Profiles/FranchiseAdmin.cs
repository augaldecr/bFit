using bFit.WEB.Data.Entities;
using bFit.WEB.Data.Entities.Profiles;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Profiles
{
    public class FranchiseAdmin : IFranchiseEmployee
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Usuario")]
        public User User { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public Franchise Franchise { get; set; }
    }
}