using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bFit.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboGoals();
        IEnumerable<SelectListItem> GetComboExercises();
        IEnumerable<SelectListItem> GetComboSubSetTypes();
        IEnumerable<SelectListItem> GetComboGenders();
        IEnumerable<SelectListItem> GetComboTowns(); 
        IEnumerable<SelectListItem> GetComboGyms(int? id);
        IEnumerable<SelectListItem> GetComboTrainers(int? franchiseId);
    }
}