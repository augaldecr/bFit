using bFit.Web.Data;
using bFit.Web.Data.Entities;
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
                Gyms = _combosHelper.GetComboGyms(customer.Gym.Id),
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
    }
}