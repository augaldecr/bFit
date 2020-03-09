using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Models;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Customer> ToCustomerAsync(CustomerViewModel model);
        Task<CustomerViewModel> ToCustomerViewModelAsync(Customer customer);
        WorkoutViewModel ToWorkoutViewModel(WorkoutRoutine workout);
        EditSubSetViewModel ToEditSubSetViewModel(SubSet subSet);
        Task<SubSet> ToSubSetAsync(EditSubSetViewModel editSubSetVwm);
    }
}