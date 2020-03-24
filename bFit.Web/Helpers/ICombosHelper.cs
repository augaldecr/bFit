using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboGoals();
        IEnumerable<SelectListItem> GetComboExercises();
        IEnumerable<SelectListItem> GetComboSubSetTypes();
        IEnumerable<SelectListItem> GetComboGenders();
        IEnumerable<SelectListItem> GetComboTowns();
        Task<IEnumerable<SelectListItem>> GetComboGymsAsync(int? id);
        IEnumerable<SelectListItem> GetComboTrainers(int? franchiseId);
        IEnumerable<SelectListItem> GetComboFranchises();
    }
}