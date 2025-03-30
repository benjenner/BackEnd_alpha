using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Services
{
    // Klistra in "RoleManager<IdentityRole> roleManager"
    public class AuthService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signManager,
        RoleManager<IdentityRole> roleManager,
        JwtTokenHandler jwtTokenHandler
        ) : IAuthService

    {
        // userManager och signInManager innehåller repository med färdig funktionalitet för att kommunicera med databasen
        private readonly UserManager<AppUser> _userManager = userManager;

        private readonly SignInManager<AppUser> _signInManager = signManager;

        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        private readonly JwtTokenHandler _jwtTokenHandler = jwtTokenHandler;

        public async Task<ServiceResult> SignUpAsync(SignUpForm form)
        {
            // Validerar att email inte existerar
            if (await _userManager.Users.AnyAsync(x => x.Email == form.Email))
                return ServiceResult.AlreadyExists("Email already exists");

            // *** Instansiera upp med AppUserFactory ***
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

            var identityResult = await _userManager.CreateAsync(appUser, form.Password);
            if (identityResult.Succeeded)
            {
                try
                {
                    // Om rollen är null, sätt roll till User
                    var userRole = form.Role ?? "User";

                    if (!await _roleManager.RoleExistsAsync(form.Role!))
                        userRole = "User";

                    var result = await _userManager.AddToRoleAsync(appUser, userRole);
                    if (result.Succeeded)
                        return ServiceResult.Ok();

                    return ServiceResult.Ok("User  was created but not assigned a role");
                }
                catch (Exception ex)
                {
                    return ServiceResult.Failed(ex.Message);
                }
            }

            return ServiceResult.Failed("Unable to create user");
        }

        public async Task<ServiceResult> SignInAsync(SignInForm form)
        {
            // PasswordSignIn skapar en cookie som lagras på servern.
            // Metoden CheckPasswordSignInAsync lagrar
            var user = await _userManager.FindByEmailAsync(form.Email);
            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, form.Password, false);
                if (signInResult.Succeeded)
                {
                    var role = (await _userManager.GetRolesAsync(user)).First();
                    var token = _jwtTokenHandler.GenerateToken(user!, role);
                    // Returnerar token
                    return ServiceResult.Ok(token);
                }
                else
                {
                    ServiceResult.Unauthorized("Wrong email or password");
                }
            }

            return ServiceResult.Unauthorized("Wrong email or password");
        }

        // Funktion som returnerar användare med hjälp av id genom usermanager
    }
}