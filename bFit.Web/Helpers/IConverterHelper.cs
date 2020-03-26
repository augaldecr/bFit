using bFit.Web.Data.Entities.Common;
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
        Task<Admin> ToAdminAsync(AdminViewModel model);
        AdminViewModel ToAdminViewModelAsync(Admin admin);
        Task<FranchiseAdmin> ToFranchiseAdminAsync(CreateFranchiseAdminViewModel franchiseAdmin);
        Task<FranchiseAdmin> ToFranchiseAdminAsync(FranchiseAdminViewModel model);
        FranchiseAdminViewModel ToFranchiseAdminViewModel(FranchiseAdmin franchiseAdmin);
        Task<GymAdmin> ToGymAdminAsync(CreateGymAdminViewModel gymAdminModel);
        Task<GymAdmin> ToGymAdminAsync(GymAdminViewModel gymAdminModel);
        Task<GymAdminViewModel> ToGymAdminViewModelAsync(GymAdmin gymAdmin, int? franchise);
        Task<Trainer> ToTrainerAsync(CreateTrainerViewModel trainerModel);
        Task<Trainer> ToTrainerAsync(TrainerViewModel model);
        Task<TrainerViewModel> ToTrainerViewModelAsync(Trainer trainer, int? franchise);
        EditGymViewModel ToEditGymViewModel(LocalGym localGym);
        Task<LocalGym> ToLocalGym(CreateGymViewModel gymView);
        Task<LocalGym> ToLocalGymAsync(EditGymViewModel gymView);
        Task<State> ToState(CreateStateViewModel stateViewModel);
    }
}