using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Interfaces
{
    public interface IAppUserService
    {
        Task<TokenResult> SignInAsync(SignInForm form);

        Task<ServiceResult> SignOutAsync();

        Task<ServiceResult> SignUpAsync(UserRegistrationForm form);

        Task<ServiceResult> CreateAppUserAsync(CreateUserForm form);

        Task<ServiceResult> DeleteAppUserAsync(string id);

        Task<ServiceResult> UpdateAppUserAsync(UpdateUserForm form);

        Task<IEnumerable<AppUser>> GetAllUsersAsync();

        Task<AppUser?> GetUserByIdAsync(string id);

        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}