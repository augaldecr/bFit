using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class GymAdminViewModel : UserViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Gimnasio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un gimnasio")]
        public int GymId { get; set; }

        public IEnumerable<SelectListItem> Gyms { get; set; }
    }
}
