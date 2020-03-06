using bFit.Web.Data;
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

        public CustomerViewModel ToCustomerViewModel(Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,
                SocialSecurity = customer.User.SocialSecurity,
                FirstName = customer.User.FirstName,
                LastName1 = customer.User.LastName1,
                LastName2 = customer.User.LastName2,
                CellPhone = customer.User.PhoneNumber,
                Email = customer.User.Email,
                Birthday = customer.Birthday,
                GenderId = customer.Gender.Id,
                TownId = customer.User.Town.Id,
                Genders = _combosHelper.GetComboGenders(),
                Towns = _combosHelper.GetComboGenders(),
                Address = customer.User.Address,
            };
        }

        public Customer ToCustomer(CustomerViewModel model)
        {
            return new Customer
            {
                Id = model.Id,
                Birthday = model.Birthday,
                //Gender = model.Gender,
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
                GoalId = workout.Goal.Id,
                Goals = _combosHelper.GetComboGoals(),
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
    }
}