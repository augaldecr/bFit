using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCountiesAsync(int id);

        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();

        IEnumerable<SelectListItem> GetComboCustomers(ICollection<Customer> customers);

        Task<IEnumerable<SelectListItem>> GetComboDistrictsAsync(int id);

        Task<IEnumerable<SelectListItem>> GetComboExercisesAsync();

        Task<IEnumerable<SelectListItem>> GetComboExerciseTypesAsync();

        Task<IEnumerable<SelectListItem>> GetComboFranchisesAsync();

        Task<IEnumerable<SelectListItem>> GetComboGendersAsync();

        Task<IEnumerable<SelectListItem>> GetComboGoalsAsync();
        Task<IEnumerable<SelectListItem>> GetComboGymsAsync(int? id);

        Task<IEnumerable<SelectListItem>> GetComboSomatypesAsync();

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int id);

        Task<IEnumerable<SelectListItem>> GetComboSubSetTypesAsync();
        public IEnumerable<SelectListItem> GetComboTemplatesAsync(ICollection<Template> Templates);

        Task<IEnumerable<SelectListItem>> GetComboTownsAsync(int districtId);

        Task<IEnumerable<SelectListItem>> GetComboTrainersAsync(int? franchiseId);
    }
}