using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bFit.Web.Models
{
    public class EditTemplateViewModel : CreateTemplateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Plantilla")]
        [HiddenInput(DisplayValue = false)]
        public int TemplateId { get; set; }
    }
}
