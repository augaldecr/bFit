using bFit.WEB.Data.Entities.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Profiles
{
    public class LocalGym : IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Número de teléfono")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Correo electrónico")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Franquicia")]
        public Franchise Franchise { get; set; }

        //Ciudad, pueblo, villa, etc..
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Localidad")]
        public Town Town { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        public string Name => $"{Franchise.TradeName} - {Town.Name}";

        [Display(Name = "Entrenadores")]
        public virtual ICollection<Trainer> Trainers { get; set; }

        [Display(Name = "Clientes")]
        public virtual ICollection<Customer> Customers { get; set; }

        [Display(Name = "Administradores de gimnasio")]
        public virtual ICollection<GymAdmin> GymAdmins { get; set; }
    }
}