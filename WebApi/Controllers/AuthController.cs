using Authentication.Interfaces;
using Authentication.Models;
using Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService)
        : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(SignUpForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await _authService.SignUpAsync(form);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignInAsync(SignInForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await _authService.SignInAsync(form);
            if (result.Succeeded)
            {
                // Message innehåller token som string
                return Ok(result.Message);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}