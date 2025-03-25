using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(ClientService clientService) : ControllerBase
    {
        private readonly ClientService _clientService = clientService;

        // Get
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientRegistrationForm form)
        {
            // Validerar mot modellen ClientRegistrationForm
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _clientService.CreateAsync(form);
            return (object)result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                409 => Conflict(result),
                _ => Problem(),
            };
        }

        // Put

        // Delete
    }
}