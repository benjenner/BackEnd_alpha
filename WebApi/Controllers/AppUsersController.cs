using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Documentation.AppUserEndpoint;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AppUsersController(IAppUserService appUserService)
        : ControllerBase
    {
        private readonly IAppUserService _appUserService = appUserService;

        [UseAdminApiKey]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all users", Description = "Only admins can retrieve users. This will require a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerResponse(200, "Returns all users", typeof(IEnumerable<AppUser>))]
        public async Task<IActionResult> GetAll()
        {
            var users = await _appUserService.GetAllUsersAsync();

            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [UseAdminApiKey]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a specific user by ID", Description = "Only admins can retrieve a user. This will require a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerResponse(200, "Returns a user by ID", typeof(AppUser))]
        [SwaggerResponse(404, "User not found", typeof(ErrorMessage))]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _appUserService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signup")]
        [SwaggerOperation(Summary = "Sign Up as a new User")]
        [SwaggerRequestExample(typeof(UserRegistrationForm), typeof(UserRegistrationFormExample))]
        [SwaggerResponseExample(409, typeof(UserExistsErrorExample))]
        [SwaggerResponse(200, "User successfully created")]
        [SwaggerResponse(409, "User already exists", typeof(ErrorMessage))]
        public async Task<IActionResult> SignUp(UserRegistrationForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(form);
            }

            var result = await appUserService.SignUpAsync(form);
            return HandleServiceResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signin")]
        [SwaggerOperation(Summary = "Sign In with a existing User")]
        [SwaggerRequestExample(typeof(SignInForm), typeof(SignInExample))]
        [SwaggerResponseExample(401, typeof(SignInErrorExample))]
        [SwaggerResponse(200, "User successfully authenticated")]
        [SwaggerResponse(401, "Invalid email or password", typeof(ErrorMessage))]
        public async Task<IActionResult> SignInAsync(SignInForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(form);
            }

            var result = await appUserService.SignInAsync(form);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [UseAdminApiKey]
        [Route("create")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Create a new User", Description = "Only admins can create users. Requires a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerRequestExample(typeof(CreateUserForm), typeof(CreateUserFormExample))]
        [SwaggerResponseExample(409, typeof(UserExistsErrorExample))]
        [SwaggerResponse(200, "User successfully created")]
        [SwaggerResponse(409, "User already exists", typeof(ErrorMessage))]
        public async Task<IActionResult> CreateUserAsync(CreateUserForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(form);
            }

            var result = await appUserService.CreateAppUserAsync(form);
            return HandleServiceResult(result);
        }

        [HttpPut]
        [UseAdminApiKey]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Update a User", Description = "Only admins can update users. Requires a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerRequestExample(typeof(UpdateUserForm), typeof(UpdateUserExample))]
        [SwaggerResponseExample(200, typeof(UserExample))]
        [SwaggerResponseExample(404, typeof(UserNotFoundExample))]
        [SwaggerResponse(200, "User successfully updated", typeof(UserExample))]
        [SwaggerResponse(404, "User not found", typeof(ErrorMessage))]
        public async Task<IActionResult> Update(UpdateUserForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _appUserService.UpdateAppUserAsync(form);
            return HandleServiceResult(result);
        }

        [HttpDelete("{Id}")]
        [UseAdminApiKey]
        [SwaggerOperation(Summary = "Delete a User", Description = "Only admins can delete users. Requires a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerResponse(200, "User successfully deleted")]
        [SwaggerResponse(404, "User not found", typeof(ErrorMessage))]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _appUserService.DeleteAppUserAsync(Id);
            return HandleServiceResult(result);
        }

        [HttpGet("roles")]
        [UseAdminApiKey]
        [SwaggerOperation(Summary = "Get all roles", Description = "Only admins can retrieve roles. Requires a API-key 'X-ADM-API-KEY' in the header request.")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _appUserService.GetAllRolesAsync();

            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles);
        }

        [HttpGet("signout")]
        public async Task<IActionResult> Signout()
        {
            var result = await _appUserService.SignOutAsync();

            return HandleServiceResult(result);
        }

        private IActionResult HandleServiceResult(ServiceResult result)
        {
            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                401 => Unauthorized(result),
                404 => NotFound(result),
                409 => Conflict(result),
                _ => Problem(result.Message, statusCode: result.StatusCode)
            };
        }
    }
}