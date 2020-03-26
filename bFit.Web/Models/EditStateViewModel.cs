using Microsoft.AspNetCore.Mvc;

namespace bFit.Web.Models
{
    public class EditStateViewModel : CreateStateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}