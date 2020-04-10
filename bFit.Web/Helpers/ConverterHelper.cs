using bFit.Web.Data;
using bFit.Web.Data.Entities;
using bFit.Web.Data.Entities.Common;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Data.Entities.Workouts;
using bFit.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;
        private readonly ApplicationDbContext _context;
        public ConverterHelper(ApplicationDbContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
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
        public async Task<AdminViewModel> ToAdminViewModelAsync(Admin admin)
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
                Countries = await _combosHelper.GetComboCountriesAsync()
            };
        }
        public async Task<County> ToCountyAsync(CreateCountyViewModel model)
        {
            return new County
            {
                Name = model.Name,
                State = await _context.States.FirstOrDefaultAsync(s => s.Id == model.StateId),
            };
        }
        public async Task<County> ToCountyAsync(EditCountyViewModel model)
        {
            var county = await _context.Counties.FirstOrDefaultAsync(c => c.Id == model.Id);
            county.Name = model.Name;
            county.State = await _context.States.FirstOrDefaultAsync(s => s.Id == model.StateId);
            return county;
        }
        public async Task<CreateStateViewModel> ToCreateStateViewModelAsync(State state)
        {
            return new CreateStateViewModel
            {
                Countries = await _combosHelper.GetComboCountriesAsync(),
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
                Somatotype = await _context.Somatotypes.FindAsync(model.SomatotypeId),

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
                .Include(c => c.Somatotype)
                .Include(c => c.User)
                    .ThenInclude(u => u.Town)
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            customer.Birthday = model.Birthday;
            customer.Somatotype = await _context.Somatotypes.FindAsync(model.SomatotypeId);
            customer.Gender = await _context.Genders.FindAsync(model.GenderId);
            customer.User.Address = model.Address;
            customer.User.Address = model.Address;
            customer.User.FirstName = model.FirstName;
            customer.User.LastName1 = model.LastName1;
            customer.User.LastName2 = model.LastName2;
            customer.User.PhoneNumber = model.CellPhone;
            customer.User.Town = await _context.Towns.FindAsync(model.TownId);

            return customer;
        }
        public async Task<CustomerViewModel> ToCustomerViewModelAsync(Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.User.FirstName,
                LastName1 = customer.User.LastName1,
                LastName2 = customer.User.LastName2,
                CellPhone = customer.User.PhoneNumber,
                Birthday = customer.Birthday,
                GenderId = customer.Gender.Id,
                SomatotypeId = customer.Somatotype.Id,
                TownId = customer.User.Town.Id,
                Genders = await _combosHelper.GetComboGendersAsync(),
                Somatotypes = await _combosHelper.GetComboSomatypesAsync(),
                Countries = await _combosHelper.GetComboCountriesAsync(),
                Address = customer.User.Address,
            };
        }
        public async Task<District> ToDistrictAsync(CreateDistrictViewModel model)
        {
            return new District
            {
                Name = model.Name,
                County = await _context.Counties.FirstOrDefaultAsync(s => s.Id == model.CountyId),
            };
        }
        public async Task<District> ToDistrictAsync(EditDistrictViewModel model)
        {
            var district = await _context.Districts.FirstOrDefaultAsync(c => c.Id == model.Id);
            district.Name = model.Name;
            district.County = await _context.Counties.FirstOrDefaultAsync(s => s.Id == model.CountyId);
            return district;
        }
        public async Task<EditCountyViewModel> ToEditCountyViewModelAsync(County county)
        {
            return new EditCountyViewModel
            {
                Id = county.Id,
                Name = county.Name,
                StateId = county.State.Id,
                States = await _combosHelper.GetComboStatesAsync(county.State.Id),
                CountryId = county.State.Country.Id,
                Countries = await _combosHelper.GetComboCountriesAsync(),
            };
        }
        public async Task<EditDistrictViewModel> ToEditDistrictViewModelAsync(District district)
        {
            var districtVwm = await _context.Districts
                .Include(d => d.County)
                    .ThenInclude(c => c.State)
                        .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(d => d.Id == district.Id);

            return new EditDistrictViewModel
            {
                Id = districtVwm.Id,
                Name = districtVwm.Name,
                CountyId = districtVwm.County.Id,
                Counties = await _combosHelper.GetComboCountiesAsync(districtVwm.County.State.Id),
                StateId = districtVwm.County.State.Id,
                States = await _combosHelper.GetComboStatesAsync(districtVwm.County.State.Id),
                CountryId = districtVwm.County.State.Country.Id,
                Countries = await _combosHelper.GetComboCountriesAsync(),
            };
        }
        public async Task<EditExerciseViewModel> ToEditExerciseViewModel(Exercise exercise)
        {
            var ex = await _context.Exercises
                .Include(e => e.ExerciseType)
                .FirstOrDefaultAsync(e => e.Id == exercise.Id);

            return new EditExerciseViewModel
            {
                Id = ex.Id,
                Name = ex.Name,
                ExerciseTypeId = ex.ExerciseType.Id,
                ExerciseTypes = await _combosHelper.GetComboExerciseTypesAsync(),
                ImageUrl = exercise.ImageUrl,
                VideoUrl = exercise.VideoUrl,
            };
        }
        public async Task<EditGymViewModel> ToEditGymViewModelAsync(LocalGym localGym)
        {
            return new EditGymViewModel
            {
                Id = localGym.Id,
                Address = localGym.Address,
                Email = localGym.Email,
                PhoneNumber = localGym.PhoneNumber,
                CountryId = localGym.Town.Id,
                Countries = await _combosHelper.GetComboCountriesAsync(),
                StateId = localGym.Town.Id,
                States = await _combosHelper
                    .GetComboStatesAsync(localGym.Town.District.County.State.Country.Id),
                CountyId = localGym.Town.Id,
                Counties = await _combosHelper.GetComboCountiesAsync(localGym.Town.District.County.State.Id),
                DistrictId = localGym.Town.Id,
                Districts = await _combosHelper.GetComboDistrictsAsync(localGym.Town.District.Id),
                TownId = localGym.Town.Id,
                Towns = await _combosHelper.GetComboTownsAsync(localGym.Town.Id),
            };
        }
        public async Task<EditStateViewModel> ToEditStateViewModelAsync(State state)
        {
            return new EditStateViewModel
            {
                Id = state.Id,
                Name = state.Name,
                CountryId = state.Country.Id,
                Countries = await _combosHelper.GetComboCountriesAsync(),
            };
        }
        public async Task<EditTemplateViewModel> ToEditTemplateViewModelAsync(Template template)
        {
            return new EditTemplateViewModel
            {
                Id = template.Id,
                TemplateId = template.Id,
                Name = template.Name,
                GoalId = template.Goal.Id,
                Goals = await _combosHelper.GetComboGoalsAsync(),
                SetTemplates = template.SetTemplates,
                FranchiseId = template.Franchise.Id,
            };
        }
        public async Task<EditTownViewModel> ToEditTownViewModelAsync(Town town)
        {
            var townVwm = await _context.Towns
                .Include(t => t.District)
                    .ThenInclude(d => d.County)
                        .ThenInclude(c => c.State)
                            .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(d => d.Id == town.Id);

            return new EditTownViewModel
            {
                Id = town.Id,
                Name = town.Name,
                DistrictId = town.District.Id,
                Districts = await _combosHelper.GetComboDistrictsAsync(town.District.County.Id),
                CountyId = town.District.County.Id,
                Counties = await _combosHelper.GetComboCountiesAsync(town.District.County.State.Id),
                StateId = town.District.County.State.Id,
                States = await _combosHelper.GetComboStatesAsync(town.District.County.State.Country.Id),
                CountryId = town.District.County.State.Country.Id,
                Countries = await _combosHelper.GetComboCountriesAsync(),
            };
        }
        public async Task<EditWorkoutViewModel> ToEditWorkoutViewModelAsync(WorkoutRoutine workout)
        {
            return new EditWorkoutViewModel
            {
                Id = workout.Id,
                Begins = workout.Begins,
                Ends = workout.Ends,
                Sets = workout.Sets,
                CustomerId = workout.Customer.Id,
                TrainerId = workout.Trainer.Id,
                Trainers = await _combosHelper.GetComboTrainersAsync(null),
                GoalId = workout.Goal.Id,
                Goals = await _combosHelper.GetComboGoalsAsync(),
                Exercises = await _combosHelper.GetComboExercisesAsync(),
                SubSetTypes = await _combosHelper.GetComboSubSetTypesAsync(),
                WorkoutId = workout.Id,
                Customer = workout.Customer,
            };
        }
        public async Task<Exercise> ToExerciseAsync(ExerciseViewModel model)
        {
            return new Exercise
            {
                Name = model.Name,
                ExerciseType = await _context.ExerciseTypes.FindAsync(model.ExerciseTypeId),
                ImageUrl = model.ImageUrl,
                VideoUrl = model.VideoUrl,
            };
        }
        public async Task<Exercise> ToExerciseAsync(EditExerciseViewModel model)
        {
            var exercise = await _context.Exercises.FindAsync(model.Id);
            exercise.ExerciseType = await _context.ExerciseTypes.FindAsync(model.ExerciseTypeId);
            exercise.Name = model.Name;
            exercise.ImageUrl = model.ImageUrl;
            exercise.VideoUrl = model.VideoUrl;
            return exercise;
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
        public async Task<FranchiseAdminViewModel> ToFranchiseAdminViewModelAsync(FranchiseAdmin franchiseAdmin)
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
                Countries = await _combosHelper.GetComboCountriesAsync(),
                FranchiseId = franchiseAdmin.Franchise.Id,
                Franchises = await _combosHelper.GetComboFranchisesAsync(),
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
                Countries = await _combosHelper.GetComboCountriesAsync(),
                GymId = gymAdmin.LocalGym.Id,
                Gyms = await _combosHelper.GetComboGymsAsync(franchise),
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
        public async Task<LocalGym> ToLocalGymAsync(CreateGymViewModel gymView)
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
        public async Task<Set> ToSetAsync(SetViewModel setView)
        {
            return new Set
            {
                Id = setView.Id,
                WorkoutRoutine = await _context.WorkoutRoutines.FirstOrDefaultAsync(w => w.Id == setView.WorkoutId),
                SubSets = setView.Subsets,
            };
        }
        public async Task<Set> ToSetAsync(SetTemplate setView)
        {
            ICollection<SubSet> subsets = new List<SubSet>();
                
             foreach (var item in setView.SubSetTemplates)
            {
                subsets.Add(await ToSubSetAsync(item));
            }

            return new Set
            {
                Id = setView.Id,
                WorkoutRoutine = await _context.WorkoutRoutines.FirstOrDefaultAsync(w => w.Id == setView.Template.Id),
                SubSets = subsets,
            };
        }
        public async Task<SetTemplateViewModel> ToSetTemplateViewModelAsync(SetTemplate set)
        {
            return new SetTemplateViewModel
            {
                Id = set.Id,
                TemplateId = set.Template.Id,
                Exercises = await _combosHelper.GetComboExercisesAsync(),
                SubSetTypes = await _combosHelper.GetComboSubSetTypesAsync(),
                SubSetTemplates = set.SubSetTemplates,
            };
        }
        public async Task<SetViewModel> ToSetViewModelAsync(Set set)
        {
            return new SetViewModel
            {
                Id = set.Id,
                WorkoutId = set.WorkoutRoutine.Id,
                Exercises = await _combosHelper.GetComboExercisesAsync(),
                SubSetTypes = await _combosHelper.GetComboSubSetTypesAsync(),
                Subsets = set.SubSets,
            };
        }
        public async Task<State> ToStateAsync(CreateStateViewModel stateViewModel)
        {
            return new State
            {
                Name = stateViewModel.Name,
                Country = await _context.Countries.FirstOrDefaultAsync(f => f.Id == stateViewModel.CountryId),
            };
        }
        public async Task<State> ToStateAsync(EditStateViewModel model)
        {
            var state = await _context.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            state.Name = model.Name;
            state.Country = await _context.Countries.FirstOrDefaultAsync(f => f.Id == model.CountryId);

            return state;
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
                .Include(s => (WorkoutRoutine)s.WorkoutRoutine)
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
        public async Task<SubSet> ToSubSetAsync(SubSetTemplate subSetView)
        {
            return new SubSet
            {
                Id = subSetView.Id,
                Exercise = subSetView.Exercise,
                PositiveTime = subSetView.PositiveTime,
                NegativeTime = subSetView.NegativeTime,
                Quantity = subSetView.Quantity,
                Remarks = subSetView.Remarks,
                SubSetType = await _context.SubSetTypes.FirstOrDefaultAsync(s => s.Id == subSetView.SubSetType.Id),
            };
        }
        public async Task<SubSetTemplate> ToSubSetTemplateAsync(SubSetTemplateViewModel subSetView)
        {
            return new SubSetTemplate
            {
                Id = subSetView.Id,
                Exercise = await _context.Exercises.FindAsync(subSetView.ExerciseId),
                NegativeTime = subSetView.NegativeTime,
                PositiveTime = subSetView.PositiveTime,
                Quantity = subSetView.Quantity,
                Remarks = subSetView.Remarks,
                SetTemplate = await _context.SetTemplates.FindAsync(subSetView.SetTemplateId),
                SubSetType = await _context.SubSetTypes.FindAsync(subSetView.SubSetTypeId),
            };
        }
        public async Task<SubSetTemplateViewModel> ToSubSetTemplateViewModel(SubSetTemplate subSet)
        {
            return new SubSetTemplateViewModel
            {
                Id = subSet.Id,
                TemplateId = subSet.SetTemplate.Template.Id,
                SetTemplateId = subSet.SetTemplate.Id,
                ExerciseId = subSet.Exercise.Id,
                Exercises = await _combosHelper.GetComboExercisesAsync(),
                NegativeTime = subSet.NegativeTime,
                PositiveTime = subSet.PositiveTime,
                Quantity = subSet.Quantity,
                Remarks = subSet.Remarks,
                SubSetTypeId = subSet.SubSetType.Id,
                SubSetTypes = await _combosHelper.GetComboSubSetTypesAsync(),
            };
        }
        public async Task<SubSetViewModel> ToSubSetViewModelAsync(SubSet subSet)
        {
            return new SubSetViewModel
            {
                Id = subSet.Id,
                WorkoutId = subSet.Set.WorkoutRoutine.Id,
                SetId = subSet.Set.Id,
                ExerciseId = subSet.Exercise.Id,
                Exercises = await _combosHelper.GetComboExercisesAsync(),
                NegativeTime = subSet.NegativeTime,
                PositiveTime = subSet.PositiveTime,
                Quantity = subSet.Quantity,
                Remarks = subSet.Remarks,
                SubSetTypeId = subSet.SubSetType.Id,
                SubSetTypes = await _combosHelper.GetComboSubSetTypesAsync(),
            };
        }
        public async Task<Template> ToTemplateAsync(CreateTemplateViewModel model)
        {
            return new Template
            {
                Name = model.Name,
                Goal = await _context.Goals.FindAsync(model.GoalId),
            };
        }
        public async Task<Template> ToTemplateAsync(EditTemplateViewModel model)
        {
            var template = await _context.Templates.FindAsync(model.Id);

            template.Id = model.Id;
            template.Name = model.Name;
            template.Goal = await _context.Goals.FindAsync(model.GoalId);
            template.Franchise = await _context.Franchises.FindAsync(
                Convert.ToInt32(model.FranchiseId));

            return template;

        }
        public async Task<Town> ToTownAsync(CreateTownViewModel model)
        {
            return new Town
            {
                Name = model.Name,
                District = await _context.Districts.FirstOrDefaultAsync(s => s.Id == model.DistrictId),
            };
        }
        public async Task<Town> ToTownAsync(EditTownViewModel model)
        {
            var town = await _context.Towns.FirstOrDefaultAsync(c => c.Id == model.Id);
            town.Name = model.Name;
            town.District = await _context.Districts.FirstOrDefaultAsync(s => s.Id == model.DistrictId);
            return town;
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
        public async Task<TrainerViewModel> ToTrainerViewModelAsync(Trainer trainer, int? franchise)
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
                Towns = await _combosHelper.GetComboTownsAsync(trainer.User.Town.District.Id),
                GymId = trainer.LocalGym.Id,
                Gyms = await _combosHelper.GetComboGymsAsync(franchise),
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
        public async Task<WorkoutRoutine> ToWorkoutAsync(AssignWorkoutToCustomerViewModel model)
        {
            var trainer = await _context.Trainers.FindAsync(model.TrainerId);
            var customer = await _context.Customers.FindAsync(model.CustomerId);

            return new WorkoutRoutine
            {
                Goal = await _context.Goals.FirstOrDefaultAsync(
                    g => g.Id == model.GoalId),
                Begins = model.Begins,
                Ends = model.Ends,
                Customer = customer,
                Trainer = trainer,
            };
        }
        public async Task<WorkoutViewModel> ToWorkoutViewModelAsync(WorkoutRoutine workout)
        {
            return new WorkoutViewModel
            {
                Id = workout.Id,
                Begins = workout.Begins,
                Ends = workout.Ends,
                Sets = workout.Sets,
                CustomerId = workout.Customer.Id,
                TrainerId = workout.Trainer.Id,
                Trainers = await _combosHelper.GetComboTrainersAsync(null),
                GoalId = workout.Goal.Id,
                Goals = await _combosHelper.GetComboGoalsAsync(),
            };
        }
    }
}