using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class FranchiseAdminViewModel : UserViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Gimnasio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un gimnasio")]
        public int FranchiseId { get; set; }

        public IEnumerable<SelectListItem> Franchises { get; set; }
    }
}
