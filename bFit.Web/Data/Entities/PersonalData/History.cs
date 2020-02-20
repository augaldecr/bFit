using bFit.WEB.Data.Entities.Profiles;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.PersonalData
{
    public class History
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Atleta")]
        public Customer Athlete { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Gimnasio")]
        public Franchise Gym { get; set; }

        public virtual ICollection<DataTake> PersonalDataTakes { get; set; }
    }
}
