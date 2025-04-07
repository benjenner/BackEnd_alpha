using Authentication.Entities;
using Authentication.Factories;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Authentication.Services
{
    public class AppUserService(UserManager<AppUserEntity> userManager,
        SignInManager<AppUserEntity> signManager,
        RoleManager<IdentityRole> roleManager,
        JwtTokenHandler jwtTokenHandler,
        IMemoryCache cache
        ) : IAppUserService

    {
        // userManager och signInManager innehåller repository med färdig funktionalitet för att kommunicera med databasen
        private readonly UserManager<AppUserEntity> _userManager = userManager;

        private readonly SignInManager<AppUserEntity> _signInManager = signManager;

        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        private readonly JwtTokenHandler _jwtTokenHandler = jwtTokenHandler;

        private readonly IMemoryCache _cache = cache;
        private const string _cacheKey_All = "Project_All";

        public async Task<ServiceResult> SignUpAsync(UserRegistrationForm form)
        {
            // Validerar att email inte existerar
            if (await _userManager.Users.AnyAsync(x => x.Email == form.Email))
            {
                return ServiceResult.AlreadyExists("Email already exists");
            }

            var appUserEntity = AppUserFactory.Map(form);

            var identityResult = await _userManager.CreateAsync(appUserEntity, form.Password);
            if (identityResult.Succeeded)
            {
                try
                {
                    // Användare sätts alltid till User
                    var result = await _userManager.AddToRoleAsync(appUserEntity, "User");
                    if (result.Succeeded)
                    {
                        _cache.Remove(_cacheKey_All);
                        return ServiceResult.Ok();
                    }

                    return ServiceResult.Ok("User  was created but not assigned a role");
                }
                catch (Exception ex)
                {
                    return ServiceResult.Failed(ex.Message);
                }
            }
            return ServiceResult.Failed("Unable to create user");
        }

        public async Task<TokenResult> SignInAsync(SignInForm form)
        {
            var user = await _userManager.FindByEmailAsync(form.Email);
            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, form.Password, false);
                if (signInResult.Succeeded)
                {
                    var role = (await _userManager.GetRolesAsync(user)).First();
                    var token = _jwtTokenHandler.GenerateToken(user!, role);
                    // Returnerar token
                    _cache.Remove(_cacheKey_All);
                    return TokenResult.Ok(token: token);
                }
                else
                {
                    TokenResult.Unauthorized(error: "Wrong email or password");
                }
            }

            return TokenResult.Unauthorized(error: "Wrong email or password");
        }

        public async Task<ServiceResult> CreateAppUserAsync(CreateUserForm form)
        {
            // Validerar att email inte existerar
            if (await _userManager.Users.AnyAsync(x => x.Email == form.Email))
            {
                return ServiceResult.AlreadyExists("Email already exists");
            }

            var appUserEntity = AppUserFactory.Map(form);

            //                                                              Sätts till ett standard-lösen då fält saknas i gränssnittet.
            var identityResult = await _userManager.CreateAsync(appUserEntity, "StandardPassword123!");
            if (identityResult.Succeeded)
            {
                try
                {
                    var result = await _userManager.AddToRoleAsync(appUserEntity, form.Role);
                    if (result.Succeeded)
                    {
                        _cache.Remove(_cacheKey_All);
                        return ServiceResult.Ok();
                    }

                    return ServiceResult.Ok("User  was created but not assigned a role");
                }
                catch (Exception ex)
                {
                    return ServiceResult.Failed(ex.Message);
                }
            }
            return ServiceResult.Failed("Unable to create user");
        }

        public async Task<ServiceResult> DeleteAppUserAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ServiceResult.BadRequest();
            }

            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return ServiceResult.NotFound();
            }

            var result = await _userManager.DeleteAsync(appUser);
            if (result.Succeeded)
            {
                _cache.Remove(_cacheKey_All);
                return ServiceResult.Ok();
            }
            return ServiceResult.Failed();
        }

        public async Task<ServiceResult> UpdateAppUserAsync(UpdateUserForm form)
        {
            if (form == null)
            {
                return ServiceResult.BadRequest();
            }

            // var appUserToUpdate = await _userManager.FindByIdAsync(form.UserId);
            var appUserToUpdate = _userManager.Users.Include(x => x.Address).SingleOrDefault(x => x.Id == form.UserId);
            if (appUserToUpdate == null)
            {
                return ServiceResult.NotFound();
            }

            var appUser = AppUserFactory.Map(form, appUserToUpdate);

            var result = await _userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
                var updatedAppUser = _userManager.Users.Include(x => x.Address).SingleOrDefault(x => x.Id == form.UserId);
                await UpdateRole(updatedAppUser, form);

                _cache.Remove(_cacheKey_All);
                return ServiceResult.Ok();
            }

            return ServiceResult.Failed();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<AppUser>? cachedItems))
            {
                return cachedItems;
            }

            _cache.Remove(_cacheKey_All);
            var entities = _userManager.Users.Include(x => x.Address)
                                       .OrderBy(x => x.FirstName).ToList();
            // Roll hämtas och tilldelas
            foreach (var entity in entities)
            {
                entity.RoleName = _userManager.GetRolesAsync(entity)
                                              .Result.FirstOrDefault();
            }

            var users = entities.Select(AppUserFactory.Map);
            _cache.Set(_cacheKey_All, users, TimeSpan.FromMinutes(10));

            return users;
        }

        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<AppUser>? cachedItems))
            {
                AppUser user = cachedItems?.FirstOrDefault(x => x.Id == id);
                if (user != null)
                    return user;
            }
            _cache.Remove(_cacheKey_All);
            var entities = _userManager.Users.Include(x => x.Address)
                                        .OrderBy(x => x.FirstName).ToList();

            foreach (var userEntity in entities)
            {
                userEntity.RoleName = _userManager.GetRolesAsync(userEntity)
                                              .Result.FirstOrDefault();
            }

            var users = entities.Select(AppUserFactory.Map);
            _cache.Set(_cacheKey_All, users, TimeSpan.FromMinutes(10));

            return users.SingleOrDefault(x => x.Id == id);
        }

        public async Task UpdateRole(AppUserEntity entity, UpdateUserForm form)
        {
            var roles = await _userManager.GetRolesAsync(entity);
            // Då mitt system endast tillåter en roll (trots att Identity tillåter flera) så fungerar
            // FirstOrDefault för att hämta ut rollen
            string role = roles.FirstOrDefault();

            if (role != form.Role)
            {
                if (role == "User")
                {
                    await _userManager.RemoveFromRoleAsync(entity, "User");
                    await _userManager.AddToRoleAsync(entity, "Admin");
                }
                else

                {
                    await _userManager.RemoveFromRoleAsync(entity, "Admin");
                    await _userManager.AddToRoleAsync(entity, "User");
                }
            }
        }

        // Funktion som hämtar samtliga roller via rolemanager
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Role>? cachedItems))
            {
                return cachedItems;
            }

            _cache.Remove(_cacheKey_All);

            var entities = await _roleManager.Roles.ToListAsync();

            var roles = entities.Select(RoleFactory.Map);
            _cache.Set(_cacheKey_All, roles, TimeSpan.FromMinutes(10));

            return roles;
        }
    }
}