using bFit.Web.Data.Entities.Common;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Models;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Admin> ToAdminAsync(CreateAdminViewModel model);

        Task<Admin> ToAdminAsync(AdminViewModel model);

        Task<AdminViewModel> ToAdminViewModelAsync(Admin admin);

        Task<County> ToCountyAsync(CreateCountyViewModel model);

        Task<County> ToCountyAsync(EditCountyViewModel model);

        Task<CreateStateViewModel> ToCreateStateViewModelAsync(State state);

        Task<Customer> ToCustomerAsync(CustomerViewModel model);
        Task<Customer> ToCustomerAsync(CreateCustomerViewModel model);
        Task<CustomerViewModel> ToCustomerViewModelAsync(Customer customer);
        Task<District> ToDistrictAsync(CreateDistrictViewModel model);

        Task<District> ToDistrictAsync(EditDistrictViewModel model);

        Task<EditCountyViewModel> ToEditCountyViewModelAsync(County county);

        Task<EditDistrictViewModel> ToEditDistrictViewModelAsync(District district);

        Task<EditExerciseViewModel> ToEditExerciseViewModel(Exercise exercise);

        Task<EditGymViewModel> ToEditGymViewModelAsync(LocalGym localGym);

        Task<EditStateViewModel> ToEditStateViewModelAsync(State state);

        Task<EditTemplateViewModel> ToEditTemplateViewModelAsync(Template template);

        Task<EditTownViewModel> ToEditTownViewModelAsync(Town town);

        Task<EditWorkoutViewModel> ToEditWorkoutViewModelAsync(WorkoutRoutine workout);

        Task<Exercise> ToExerciseAsync(ExerciseViewModel model);

        Task<Exercise> ToExerciseAsync(EditExerciseViewModel model);

        Task<FranchiseAdmin> ToFranchiseAdminAsync(CreateFranchiseAdminViewModel franchiseAdmin);

        Task<FranchiseAdmin> ToFranchiseAdminAsync(FranchiseAdminViewModel model);

        Task<FranchiseAdminViewModel> ToFranchiseAdminViewModelAsync(FranchiseAdmin franchiseAdmin);

        Task<GymAdmin> ToGymAdminAsync(CreateGymAdminViewModel gymAdminModel);

        Task<GymAdmin> ToGymAdminAsync(GymAdminViewModel gymAdminModel);

        Task<GymAdminViewModel> ToGymAdminViewModelAsync(GymAdmin gymAdmin, int? franchise);

        Task<LocalGym> ToLocalGymAsync(CreateGymViewModel gymView);

        Task<LocalGym> ToLocalGymAsync(EditGymViewModel gymView);

        Task<Set> ToSetAsync(SetViewModel setView);
        Task<Set> ToSetAsync(SetTemplate setView);

        Task<SetTemplateViewModel> ToSetTemplateViewModelAsync(SetTemplate set);

        Task<SetViewModel> ToSetViewModelAsync(Set set);

        Task<State> ToStateAsync(CreateStateViewModel stateViewModel);

        Task<State> ToStateAsync(EditStateViewModel stateViewModel);

        Task<SubSet> ToSubSetAsync(SubSetViewModel subSetView);

        Task<SubSet> ToSubSetAsync(SubSetTemplate subSetView);

        Task<SubSetTemplate> ToSubSetTemplateAsync(SubSetTemplateViewModel subSetView);

        Task<SubSetTemplateViewModel> ToSubSetTemplateViewModel(SubSetTemplate subSet);

        Task<SubSetViewModel> ToSubSetViewModelAsync(SubSet subSet);

        Task<Template> ToTemplateAsync(CreateTemplateViewModel model);

        Task<Template> ToTemplateAsync(EditTemplateViewModel model);

        Task<Town> ToTownAsync(CreateTownViewModel model);

        Task<Town> ToTownAsync(EditTownViewModel model);

        Task<Trainer> ToTrainerAsync(CreateTrainerViewModel trainerModel);

        Task<Trainer> ToTrainerAsync(TrainerViewModel model);

        Task<TrainerViewModel> ToTrainerViewModelAsync(Trainer trainer, int? franchise);

        Task<WorkoutRoutine> ToWorkoutAsync(WorkoutViewModel workoutView);

        Task<WorkoutRoutine> ToWorkoutAsync(AssignWorkoutToCustomerViewModel model);

        Task<WorkoutViewModel> ToWorkoutViewModelAsync(WorkoutRoutine workout);
    }
}