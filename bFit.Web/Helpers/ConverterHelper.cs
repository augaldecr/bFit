using bFit.Web.Data;
using bFit.Web.Data.Entities;
using bFit.Web.Data.Entities.Common;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(ApplicationDbContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<CustomerViewModel> ToCustomerViewModelAsync(Customer customer)
        {
            if (customer.Gym.Id == 0)
            {
                var gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Franchise.LegalId == "0000");
                customer.Gym.Id = gym.Id;
            }

            return new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.User.FirstName,
                LastName1 = customer.User.LastName1,
                LastName2 = customer.User.LastName2,
                CellPhone = customer.User.PhoneNumber,
                Birthday = customer.Birthday,
                GenderId = customer.Gender.Id,
                TownId = customer.User.Town.Id,
                Genders = _combosHelper.GetComboGenders(),
                Towns = _combosHelper.GetComboTowns(),
                Address = customer.User.Address,
                GymId = customer.Gym.Id,
                Gyms = await _combosHelper.GetComboGymsAsync(customer.Gym.Id),
            };
        }

        public async Task<Customer> ToCustomerAsync(CreateCustomerViewModel model)
        {
            if (model.GymId == 0)
            {
                var gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Franchise.LegalId == "0000");
                model.GymId = gym.Id;
            }
            return new Customer
            {
                Id = model.Id,
                Gender = await _context.Genders.FirstOrDefaultAsync(g => g.Id == model.GenderId),
                Birthday = model.Birthday,

                Gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == model.GymId),
                User = new User
                {
                    FirstName = model.FirstName,
                    LastName1 = model.LastName1,
                    LastName2 = model.LastName2,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.CellPhone,
                    Town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == model.TownId),
                    Address = model.Address,
                }
            };
        }

        public async Task<Customer> ToCustomerAsync(CustomerViewModel model)
        {
            var customer = await _context.Customers
                .Include(c => c.User)
                    .ThenInclude(u => u.Town)
                .Include(c => c.Gym)
                    .ThenInclude(g => g.Franchise)
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            customer.Birthday = model.Birthday;
            customer.Gender = await _context.Genders.FirstOrDefaultAsync(g => g.Id == model.GenderId);
            customer.Gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == model.GymId);
            customer.User.Address = model.Address;
            customer.User.Address = model.Address;
            customer.User.FirstName = model.FirstName;
            customer.User.LastName1 = model.LastName1;
            customer.User.LastName2 = model.LastName2;
            customer.User.PhoneNumber = model.CellPhone;
            customer.User.Town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == model.TownId);

            return customer;
        }

        public WorkoutViewModel ToWorkoutViewModel(WorkoutRoutine workout)
        {
            return new WorkoutViewModel
            {
                Id = workout.Id,
                Begins = workout.Begins,
                Ends = workout.Ends,
                Sets = workout.Sets,
                CustomerId = workout.Customer.Id,
                TrainerId = workout.Trainer.Id,
                Trainers = _combosHelper.GetComboTrainers(null),
                GoalId = workout.Goal.Id,
                Goals = _combosHelper.GetComboGoals(),
            };
        }

        public EditWorkoutViewModel ToEditWorkoutViewModel(WorkoutRoutine workout)
        {
            return new EditWorkoutViewModel
            {
                Id = workout.Id,
                Begins = workout.Begins,
                Ends = workout.Ends,
                Sets = workout.Sets,
                CustomerId = workout.Customer.Id,
                TrainerId = workout.Trainer.Id,
                Trainers = _combosHelper.GetComboTrainers(null),
                GoalId = workout.Goal.Id,
                Goals = _combosHelper.GetComboGoals(),
                Exercises = _combosHelper.GetComboExercises(),
                SubSetTypes = _combosHelper.GetComboSubSetTypes(),
                WorkoutId = workout.Id, 
                Customer = workout.Customer,
            };
        }

        public SubSetViewModel ToSubSetViewModel(SubSet subSet)
        {
            return new SubSetViewModel
            {
                Id = subSet.Id,
                WorkoutId = subSet.Set.WorkoutRoutine.Id,
                SetId = subSet.Set.Id,
                ExerciseId = subSet.Exercise.Id,
                Exercises = _combosHelper.GetComboExercises(),
                NegativeTime = subSet.NegativeTime,
                PositiveTime = subSet.PositiveTime,
                Quantity = subSet.Quantity,
                Remarks = subSet.Remarks,
                SubSetTypeId = subSet.SubSetType.Id,
                SubSetTypes = _combosHelper.GetComboSubSetTypes(),
            };
        }

        public async Task<WorkoutRoutine> ToWorkoutAsync(WorkoutViewModel workoutView)
        {
            return new WorkoutRoutine
            {
                Begins = workoutView.Begins,
                Ends = workoutView.Ends,
                Goal = await _context.Goals.FirstOrDefaultAsync(g => g.Id == workoutView.GoalId),
                Customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == workoutView.CustomerId),
                Trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id == workoutView.TrainerId),
            };
        }

        public async Task<SubSet> ToSubSetAsync(SubSetViewModel subSetView)
        {
            var exercise = await _context.Exercises
                .Include(e => e.ExerciseType)
                .FirstOrDefaultAsync(e => e.Id == subSetView.ExerciseId);

            var set = await _context.Sets
                .Include(s => s.WorkoutRoutine)
                    .ThenInclude(w => w.Customer)
                        .ThenInclude(c => c.Gender)
                .Include(s => s.WorkoutRoutine)
                    .ThenInclude(w => w.Goal)
                .Include(s => s.WorkoutRoutine)
                    .ThenInclude(w => w.Trainer)
                .FirstOrDefaultAsync(s => s.Id == subSetView.SetId);

            return new SubSet
            {
                Id = subSetView.Id,
                Exercise = exercise,
                PositiveTime = subSetView.PositiveTime,
                NegativeTime = subSetView.NegativeTime,
                Quantity = subSetView.Quantity,
                Remarks = subSetView.Remarks,
                SubSetType = await _context.SubSetTypes.FirstOrDefaultAsync(s => s.Id == subSetView.SubSetTypeId),
                Set = set,
            };
        }

        public async Task<Set> ToSetAsync(SetViewModel setView)
        {
            return new Set
            {
              Id = setView.Id,
              WorkoutRoutine = await _context.WorkoutRoutines.FirstOrDefaultAsync(w => w.Id == setView.WorkoutId),
              SubSets = setView.Subsets,
            };
        }

        public SetViewModel ToSetViewModel(Set set)
        {
            return new SetViewModel
            {
                Id = set.Id,
                WorkoutId = set.WorkoutRoutine.Id,
                Exercises = _combosHelper.GetComboExercises(),
                SubSetTypes = _combosHelper.GetComboSubSetTypes(),
                Subsets = set.SubSets,
            };
        }

        public async Task<Admin> ToAdminAsync(CreateAdminViewModel model)
        {
            return new Admin
            {
                User = new User
                {
                    Address = model.Address,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName1 = model.LastName1,
                    LastName2 = model.LastName2,
                    PhoneNumber = model.CellPhone,
                    UserName = model.Email,
                    Town = await _context.Towns.FirstOrDefaultAsync(T => T.Id == model.TownId),
                }
            };
        }

        public async Task<Admin> ToAdminAsync(AdminViewModel model)
        {
            var admin = await _context.Admins
                .Include(a => a.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            admin.User.Address = model.Address;
            admin.User.FirstName = model.FirstName;
            admin.User.LastName1 = model.LastName1;
            admin.User.LastName2 = model.LastName2;
            admin.User.PhoneNumber = model.CellPhone;
            admin.User.Town = await _context.Towns.FirstOrDefaultAsync(T => T.Id == model.TownId);

            return admin;
        }

        public AdminViewModel ToAdminViewModelAsync(Admin admin)
        {
            return new AdminViewModel
            {
                Id = admin.Id,
                Address = admin.User.Address,
                FirstName = admin.User.FirstName,
                LastName1 = admin.User.LastName1,
                LastName2 = admin.User.LastName2,
                CellPhone = admin.User.PhoneNumber,
                TownId = admin.User.Town.Id,
                Towns = _combosHelper.GetComboTowns()
            };
        }

        public async Task<FranchiseAdmin> ToFranchiseAdminAsync(CreateFranchiseAdminViewModel model)
        {
            var franchise = await _context.Franchises.FirstOrDefaultAsync(f => f.Id == model.FranchiseId);
            var town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == model.TownId);
            return new FranchiseAdmin
            {
                 Franchise = franchise,
                 User = new User
                 {
                     Address = model.Address,
                     Email = model.Email,
                     UserName = model.Email,
                     FirstName = model.FirstName,
                     LastName1 = model.LastName1,
                     LastName2 = model.LastName2,
                     PhoneNumber = model.CellPhone,
                     Town = town,
                 }
            };
        }

        public async Task<FranchiseAdmin> ToFranchiseAdminAsync(FranchiseAdminViewModel model)
        {
            var franchiseAdmin = await _context.FranchiseAdmins
                .Include(f => f.Franchise)
                .Include(f => f.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(f => f.Id == model.Id);

            var franchise = await _context.Franchises.FirstOrDefaultAsync(f => f.Id == model.FranchiseId);
            var town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == model.TownId);

            franchiseAdmin.Franchise = franchise;
            franchiseAdmin.User.Address = model.Address;
            franchiseAdmin.User.FirstName = model.FirstName;
            franchiseAdmin.User.LastName1 = model.LastName1;
            franchiseAdmin.User.LastName2 = model.LastName2;
            franchiseAdmin.User.PhoneNumber = model.CellPhone;
            franchiseAdmin.User.Town = town;

            return franchiseAdmin;
        }

        public FranchiseAdminViewModel ToFranchiseAdminViewModel(FranchiseAdmin franchiseAdmin)
        {
            return new FranchiseAdminViewModel
            {
                Id = franchiseAdmin.Id,
                Address = franchiseAdmin.User.Address,
                FirstName = franchiseAdmin.User.FirstName,
                LastName1 = franchiseAdmin.User.LastName1,
                LastName2 = franchiseAdmin.User.LastName2,
                CellPhone = franchiseAdmin.User.PhoneNumber,
                TownId = franchiseAdmin.User.Town.Id,
                Towns = _combosHelper.GetComboTowns(),
                FranchiseId = franchiseAdmin.Franchise.Id,
                Franchises = _combosHelper.GetComboFranchises(),
            };
        }

        public async Task<GymAdmin> ToGymAdminAsync(CreateGymAdminViewModel gymAdminModel)
        {
            var gym = await _context.Gyms.FirstOrDefaultAsync(f => f.Id == gymAdminModel.GymId);
            var town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == gymAdminModel.TownId);
            return new GymAdmin
            {
                LocalGym = gym,
                User = new User
                {
                    Address = gymAdminModel.Address,
                    Email = gymAdminModel.Email,
                    UserName = gymAdminModel.Email,
                    FirstName = gymAdminModel.FirstName,
                    LastName1 = gymAdminModel.LastName1,
                    LastName2 = gymAdminModel.LastName2,
                    PhoneNumber = gymAdminModel.CellPhone,
                    Town = town,
                }
            };
        }

        public async Task<GymAdmin> ToGymAdminAsync(GymAdminViewModel model)
        {
            var gymAdmin = await _context.GymAdmins
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Franchise)
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Town)
                .Include(f => f.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(f => f.Id == model.Id);

            var gym = await _context.Gyms
                .Include(g => g.Town)
                .Include(g => g.Franchise)
                .FirstOrDefaultAsync(f => f.Id == model.GymId);
            var town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == model.TownId);

            gymAdmin.LocalGym = gym;
            gymAdmin.User.Address = model.Address;
            gymAdmin.User.FirstName = model.FirstName;
            gymAdmin.User.LastName1 = model.LastName1;
            gymAdmin.User.LastName2 = model.LastName2;
            gymAdmin.User.PhoneNumber = model.CellPhone;
            gymAdmin.User.Town = town;

            return gymAdmin;
        }

        public async Task<GymAdminViewModel> ToGymAdminViewModelAsync(GymAdmin gymAdmin, int? franchise)
        {
            return new GymAdminViewModel
            {
                Id = gymAdmin.Id,
                Address = gymAdmin.User.Address,
                FirstName = gymAdmin.User.FirstName,
                LastName1 = gymAdmin.User.LastName1,
                LastName2 = gymAdmin.User.LastName2,
                CellPhone = gymAdmin.User.PhoneNumber,
                TownId = gymAdmin.User.Town.Id,
                Towns = _combosHelper.GetComboTowns(),
                GymId = gymAdmin.LocalGym.Id,
                Gyms = await _combosHelper.GetComboGymsAsync(franchise),
            };
        }

        public async Task<Trainer> ToTrainerAsync(CreateTrainerViewModel trainerModel)
        {
            var gym = await _context.Gyms.FirstOrDefaultAsync(f => f.Id == trainerModel.GymId);
            var town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == trainerModel.TownId);
            return new Trainer
            {
                LocalGym = gym,
                User = new User
                {
                    Address = trainerModel.Address,
                    Email = trainerModel.Email,
                    UserName = trainerModel.Email,
                    FirstName = trainerModel.FirstName,
                    LastName1 = trainerModel.LastName1,
                    LastName2 = trainerModel.LastName2,
                    PhoneNumber = trainerModel.CellPhone,
                    Town = town,
                }
            };
        }

        public async Task<Trainer> ToTrainerAsync(TrainerViewModel model)
        {
            var trainer = await _context.Trainers
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Franchise)
                .Include(f => f.LocalGym)
                    .ThenInclude(l => l.Town)
                .Include(f => f.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(f => f.Id == model.Id);

            var gym = await _context.Gyms
                .Include(g => g.Town)
                .Include(g => g.Franchise)
                .FirstOrDefaultAsync(f => f.Id == model.GymId);
            var town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == model.TownId);

            trainer.LocalGym = gym;
            trainer.User.Address = model.Address;
            trainer.User.FirstName = model.FirstName;
            trainer.User.LastName1 = model.LastName1;
            trainer.User.LastName2 = model.LastName2;
            trainer.User.PhoneNumber = model.CellPhone;
            trainer.User.Town = town;

            return trainer;
        }

        public async Task<TrainerViewModel> ToTrainerViewModelAsync(Trainer trainer , int? franchise)
        {
            return new TrainerViewModel
            {
                Id = trainer.Id,
                Address = trainer.User.Address,
                FirstName = trainer.User.FirstName,
                LastName1 = trainer.User.LastName1,
                LastName2 = trainer.User.LastName2,
                CellPhone = trainer.User.PhoneNumber,
                TownId = trainer.User.Town.Id,
                Towns = _combosHelper.GetComboTowns(),
                GymId = trainer.LocalGym.Id,
                Gyms = await _combosHelper.GetComboGymsAsync(franchise),
            };
        }

        public EditGymViewModel ToEditGymViewModel(LocalGym localGym)
        {
            return new EditGymViewModel
            {
                Id = localGym.Id,
                Address = localGym.Address,
                Email = localGym.Email,
                PhoneNumber = localGym.PhoneNumber,
                TownId = localGym.Town.Id,
                Towns = _combosHelper.GetComboTowns(),
            };
        }

        public async Task<LocalGym> ToLocalGymAsync(EditGymViewModel gymView)
        {
            var gym = await _context.Gyms
                .Include(g => g.Franchise)
                .Include(g => g.Town)
                .FirstOrDefaultAsync(g => g.Id == gymView.Id);

            gym.Address = gymView.Address;
            gym.Email = gymView.Email;
            gym.PhoneNumber = gymView.PhoneNumber;
            gym.Town = await _context.Towns.FindAsync(gymView.TownId);

            return gym;
        }

        public async Task<LocalGym> ToLocalGym(CreateGymViewModel gymView)
        {
            return new LocalGym
            {
                Address = gymView.Address,
                Email = gymView.Email,
                PhoneNumber = gymView.PhoneNumber,
                Town = await _context.Towns.FindAsync(gymView.TownId),
                Franchise = await _context.Franchises.FirstOrDefaultAsync(f => f.Id == gymView.FranchiseId),
            };
        }

        public async Task<State> ToState(CreateStateViewModel stateViewModel)
        {
            return new State
            {
                Name = stateViewModel.Name,
                Country = await _context.Countries.FirstOrDefaultAsync(f => f.Id == stateViewModel.CountryId),
            };
        }

        public async Task<EditStateViewModel> ToEditStateViewModel (State state)
        {
            return new EditStateViewModel
            {
                Name = stateViewModel.Name,
                Country = await _context.Countries.FirstOrDefaultAsync(f => f.Id == state.CountryId),
            };
        }
    }
}