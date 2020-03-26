using bFit.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace bFit.Web.Models
{
    public class EditGymViewModel : GymViewModel, IEntity
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}
