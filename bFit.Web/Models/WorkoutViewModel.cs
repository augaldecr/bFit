using bFit.Web.Data.Entities;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class WorkoutViewModel : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CustomerId { get; set; }

        [Display(Name = "Entrenador")]
        [HiddenInput(DisplayValue = false)]
        public int TrainerId { get; set; }

        public Customer Customer { get; set; }

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

        public IEnumerable<SelectListItem> Trainers { get; set; }

        public virtual ICollection<Set> Sets { get; set; }
    }
}
