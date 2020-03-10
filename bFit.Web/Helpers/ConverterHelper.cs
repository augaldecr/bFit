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
                Email = customer.User.Email,
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

        public async Task<Customer> ToCustomerAsync(CustomerViewModel model)
        {
            if (model.GymId == 0)
            {
                var gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Franchise.LegalId == "0000");
                model.GymId = gym.Id;
            }
            return new Customer
            {
                Id = model.Id,
                Gender =await  _context.Genders.FirstOrDefaultAsync(g => g.Id == model.GenderId),
                Birthday = model.Birthday,
                
                Gym = await _context.Gyms.FirstOrDefaultAsync(g => g.Id == model.GymId),
                User = new User {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName1 = model.LastName1,
                    LastName2 = model.LastName2,
                    PhoneNumber = model.CellPhone,
                    Town = await _context.Towns.FirstOrDefaultAsync(t => t.Id == model.TownId),
                    Address = model.Address,
                }
            };
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
            };
        }

        public EditSubSetViewModel ToEditSubSetViewModel(SubSet subSet)
        {
            return new EditSubSetViewModel
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

        public async Task<SubSet> ToSubSetAsync(EditSubSetViewModel editSubSetVwm)
        {
            return new SubSet
            {
                Id = editSubSetVwm.Id,
                Exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == editSubSetVwm.ExerciseId),
                PositiveTime = editSubSetVwm.PositiveTime,
                NegativeTime = editSubSetVwm.NegativeTime,
                Quantity = editSubSetVwm.Quantity,
                Remarks = editSubSetVwm.Remarks,
                Set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == editSubSetVwm.SetId),
                SubSetType = await _context.SubSetTypes.FirstOrDefaultAsync(s => s.Id == editSubSetVwm.SubSetTypeId),
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
    }
}