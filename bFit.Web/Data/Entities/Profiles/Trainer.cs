﻿using bFit.WEB.Data.Entities.Workouts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Profiles
{
    public class Trainer : IEntity, IGymEmployee
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Usuario")]
        public User User { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Gimnasio")]
        public LocalGym Gym { get; set; }
    }
}