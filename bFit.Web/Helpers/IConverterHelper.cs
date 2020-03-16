using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Models;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Customer> ToCustomerAsync(CustomerViewModel model);
        Task<Customer> ToCustomerAsync(CreateCustomerViewModel model);
        Task<CustomerViewModel> ToCustomerViewModelAsync(Customer customer);
        WorkoutViewModel ToWorkoutViewModel(WorkoutRoutine workout);
        SubSetViewModel ToSubSetViewModel(SubSet subSet);
        Task<WorkoutRoutine> ToWorkoutAsync(WorkoutViewModel workoutView);
        EditWorkoutViewModel ToEditWorkoutViewModel(WorkoutRoutine workout);
        Task<SubSet> ToSubSetAsync(SubSetViewModel subSetView);
        Task<Set> ToSetAsync(SetViewModel setView);
        SetViewModel ToSetViewModel(Set set);
        Task<Admin> ToAdminAsync(CreateAdminViewModel model);
        Task<AdminViewModel> ToAdminViewModelAsync(Admin admin);
    }
}