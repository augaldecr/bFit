using bFit.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class CustomerViewModel : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Primer apellido")]
        public string LastName1 { get; set; }

        [Display(Name = "Segundo apellido")]
        public string LastName2 { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Teléfono celular")]
        [Phone]
        public string CellPhone { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Localidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de ejecución")]
        public int TownId { get; set; }

        public IEnumerable<SelectListItem> Towns { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Género")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de ejecución")]
        public int GenderId { get; set; }

        public IEnumerable<SelectListItem> Genders { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName => $"{LastName1} {LastName2} {FirstName}";
    }
}
