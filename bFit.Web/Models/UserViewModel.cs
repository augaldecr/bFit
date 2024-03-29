﻿using bFit.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class UserViewModel : IEntity
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
        [Display(Name = "Teléfono celular")]
        [Phone]
        public string CellPhone { get; set; }

        [Display(Name = "País")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Provincia")]
        public int StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "Cantón")]
        public int CountyId { get; set; }

        public IEnumerable<SelectListItem> Counties { get; set; }

        [Display(Name = "Distrito")]
        public int DistrictId { get; set; }

        public IEnumerable<SelectListItem> Districts { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Localidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de ejecución")]
        public int TownId { get; set; }

        public IEnumerable<SelectListItem> Towns { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }
    }
}
