using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Models;

namespace bFit.Web.Helpers
{
    public interface IConverterHelper
    {
        Customer ToCustomer(CustomerViewModel model);
        CustomerViewModel ToCustomerViewModel(Customer customer);
        EditWorkoutViewModel ToEditWorkoutViewModel(WorkoutRoutine workout);
        EditSubSetViewModel ToEditSubSetViewModel(SubSet subSet);
    }
}