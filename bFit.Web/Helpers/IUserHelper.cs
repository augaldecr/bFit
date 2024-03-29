﻿using bFit.Web.Data.Entities;
using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        UserType TypeOfUser(IEntity user);
        Task<int?> GetFranchise(string email);
        Task<int?> GetGym(string email);
    }
}