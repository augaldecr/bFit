using bFit.Web.Data;
using bFit.Web.Data.Entities;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserHelper(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public UserType TypeOfUser(IEntity user)
        {
            if (user.GetType() == typeof(Admin))
            {
                return UserType.Admin;
            }
            else if (user.GetType() == typeof(FranchiseAdmin))
            {
                return UserType.FranchiseAdmin;
            }
            else if (user.GetType() == typeof(GymAdmin))
            {
                return UserType.GymAdmin;
            }
            else if (user.GetType() == typeof(Trainer))
            {
                return UserType.Trainer;
            }
            else
            {
                return UserType.Customer;
            }
        }

        public async Task<int?> GetFranchise(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return null;
            }
            else if (await _userManager.IsInRoleAsync(user, "FranchiseAdmin"))
            {
                var employee = await _context.FranchiseAdmins.FirstOrDefaultAsync(
                    a => a.User.Email == user.Email);
                return employee.Franchise.Id;
            }
            else if (await _userManager.IsInRoleAsync(user, "GymAdmin"))
            {
                var employee = await _context.GymAdmins.FirstOrDefaultAsync(
                    a => a.User.Email == user.Email);
                return employee.Franchise.Id;
            }
            else if (await _userManager.IsInRoleAsync(user, "Trainer"))
            {
                var employee = await _context.Trainers.FirstOrDefaultAsync(
                   a => a.User.Email == user.Email);
                return employee.Franchise.Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int?> GetGym(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (await _userManager.IsInRoleAsync(user, "GymAdmin"))
            {
                var employee = await _context.GymAdmins.FirstOrDefaultAsync(
                    a => a.User.Email == user.Email);
                return employee.LocalGym.Id;
            }
            else if (await _userManager.IsInRoleAsync(user, "Trainer"))
            {
                var employee = await _context.Trainers.FirstOrDefaultAsync(
                   a => a.User.Email == user.Email);
                return employee.LocalGym.Id;
            } else
            {
                return null;
            }
        }
    }
}