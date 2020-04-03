using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboGoalsAsync();
        Task<IEnumerable<SelectListItem>> GetComboExercisesAsync();
        Task<IEnumerable<SelectListItem>> GetComboSubSetTypesAsync();
        Task<IEnumerable<SelectListItem>> GetComboGendersAsync();
        Task<IEnumerable<SelectListItem>> GetComboGymsAsync(int? id);
        Task<IEnumerable<SelectListItem>> GetComboTrainersAsync(int? franchiseId);
        Task<IEnumerable<SelectListItem>> GetComboFranchisesAsync();
        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();
        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int id);
        Task<IEnumerable<SelectListItem>> GetComboCountiesAsync(int id);
        Task<IEnumerable<SelectListItem>> GetComboDistrictsAsync(int id);
        Task<IEnumerable<SelectListItem>> GetComboTownsAsync(int districtId);
    }
}