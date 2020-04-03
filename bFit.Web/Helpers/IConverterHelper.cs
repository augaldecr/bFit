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
        Task<WorkoutViewModel> ToWorkoutViewModelAsync(WorkoutRoutine workout);
        Task<SubSetViewModel> ToSubSetViewModelAsync(SubSet subSet);
        Task<WorkoutRoutine> ToWorkoutAsync(WorkoutViewModel workoutView);
        Task<EditWorkoutViewModel> ToEditWorkoutViewModelAsync(WorkoutRoutine workout);
        Task<SubSet> ToSubSetAsync(SubSetViewModel subSetView);
        Task<Set> ToSetAsync(SetViewModel setView);
        Task<SetViewModel> ToSetViewModelAsync(Set set);
        Task<Admin> ToAdminAsync(CreateAdminViewModel model);
        Task<Admin> ToAdminAsync(AdminViewModel model);
        Task<AdminViewModel> ToAdminViewModelAsync(Admin admin);
        Task<FranchiseAdmin> ToFranchiseAdminAsync(CreateFranchiseAdminViewModel franchiseAdmin);
        Task<FranchiseAdmin> ToFranchiseAdminAsync(FranchiseAdminViewModel model);
        Task<FranchiseAdminViewModel> ToFranchiseAdminViewModelAsync(FranchiseAdmin franchiseAdmin);
        Task<GymAdmin> ToGymAdminAsync(CreateGymAdminViewModel gymAdminModel);
        Task<GymAdmin> ToGymAdminAsync(GymAdminViewModel gymAdminModel);
        Task<GymAdminViewModel> ToGymAdminViewModelAsync(GymAdmin gymAdmin, int? franchise);
        Task<Trainer> ToTrainerAsync(CreateTrainerViewModel trainerModel);
        Task<Trainer> ToTrainerAsync(TrainerViewModel model);
        Task<TrainerViewModel> ToTrainerViewModelAsync(Trainer trainer, int? franchise);
        Task<EditGymViewModel> ToEditGymViewModelAsync(LocalGym localGym);
        Task<LocalGym> ToLocalGymAsync(CreateGymViewModel gymView);
        Task<LocalGym> ToLocalGymAsync(EditGymViewModel gymView);
        Task<State> ToStateAsync(CreateStateViewModel stateViewModel);
        Task<State> ToStateAsync(EditStateViewModel stateViewModel);
        Task<EditStateViewModel> ToEditStateViewModelAsync(State state);
        Task<County> ToCountyAsync(CreateCountyViewModel model);
        Task<County> ToCountyAsync(EditCountyViewModel model);
        Task<CreateStateViewModel> ToCreateStateViewModelAsync(State state);
        Task<EditCountyViewModel> ToEditCountyViewModelAsync(County county);
        Task<District> ToDistrictAsync(CreateDistrictViewModel model);
        Task<District> ToDistrictAsync(EditDistrictViewModel model);
        Task<EditDistrictViewModel> ToEditDistrictViewModelAsync(District district);
        Task<Town> ToTownAsync(CreateTownViewModel model);
        Task<Town> ToTownAsync(EditTownViewModel model);
        Task<EditTownViewModel> ToEditTownViewModelAsync(Town town);
    }
}