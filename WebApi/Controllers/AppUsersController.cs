using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController(IAppUserService appUserService)
        : ControllerBase
    {
        private readonly IAppUserService _appUserService = appUserService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _appUserService.GetAllUsersAsync();

            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _appUserService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _appUserService.GetAllRolesAsync();

            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles);
        }

        [HttpPost]
        [Route("signup")]
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
        [Route("signin")]
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
        [Route("create")]
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
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _appUserService.DeleteAppUserAsync(Id);
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