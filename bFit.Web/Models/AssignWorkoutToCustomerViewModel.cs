using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class AssignWorkoutToCustomerViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime Begins { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de fin")]
        [DataType(DataType.Date)]
        public DateTime Ends { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Meta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una meta")]
        public int GoalId { get; set; }

        public IEnumerable<SelectListItem> Goals { get; set; }

        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        [Display(Name = "Plantilla")]
        public int TemplateId { get; set; }

        [Display(Name = "Entrenador")]
        [HiddenInput(DisplayValue = false)]
        public int TrainerId { get; set; }

        public IEnumerable<SelectListItem> Trainers { get; set; }
    }
}
