using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Models;
using bFit.WEB.Data.Entities;
using bFit.WEB.Data.Entities.Profiles;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
    }
}