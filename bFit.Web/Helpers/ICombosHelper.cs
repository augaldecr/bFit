using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bFit.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboGoals();
        IEnumerable<SelectListItem> GetComboExercises();
        IEnumerable<SelectListItem> GetComboSubSetTypes();
    }
}