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
        [Display(Name = "Gimnasio")]
        public LocalGym Gym { get; set; }

        public ICollection<WorkoutRoutine> WorkOutRoutines { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName => $"{User.LastName1} {User.LastName2} {User.FirstName}";

        [Display(Name = "Dirección completa")]
        public string FullAddress => $"{User.Town.Name}, {User.Address}";
    }
}