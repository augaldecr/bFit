using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class CustomerViewModel : UserViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Género")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de ejecución")]
        public int GenderId { get; set; }

        public IEnumerable<SelectListItem> Genders { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Gimnasio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un gimnasio")]
        public int GymId { get; set; }

        public IEnumerable<SelectListItem> Gyms { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Somatotipo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un somatotipo")]
        public int SomatotypeId { get; set; }

        public IEnumerable<SelectListItem> Somatotypes { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName => $"{LastName1} {LastName2} {FirstName}";
    }
}
