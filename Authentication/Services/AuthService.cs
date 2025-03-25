using Authentication.Interfaces;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Services
{
    public class AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signManager) : IAuthService
    {
        // userManager och signInManager innehåller repository med färdig funktionalitet för att kommunicera med databasen
        private readonly UserManager<AppUser> _userManager = userManager;

        private readonly SignInManager<AppUser> _signInManager = signManager;

        public async Task<IdentityResult> SignUpAsync(SignUpForm form)
        {
            //
            var appUser = new AppUser
            {
                UserName = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email
            };

            appUser.Address = new AppUserAddress
            {
                UserId = appUser.Id,
            };

            var result = await _userManager.CreateAsync(appUser, form.Password);
            return result;
        }

        public async Task<SignInResult> SignInAsync(SignInForm form)
        {
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
            return result;
        }
    }
}