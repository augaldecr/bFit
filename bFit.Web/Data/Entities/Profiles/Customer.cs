using bFit.Web.Data.Entities.Common;
using bFit.Web.Data.Entities.Workouts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Data.Entities.Profiles
{
    public class Customer : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Usuario")]
        public User User { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Género")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Somatotipo")]
        public Somatotype Somatotype { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<WorkoutRoutine> WorkOutRoutines { get; set; }
    }
}