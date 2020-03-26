using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class CreateGymViewModel : GymViewModel
    {
        [Display(Name = "Franquicia")]
        public int FranchiseId { get; set; }

        public IEnumerable<SelectListItem> Franchises { get; set; }
    }
}