using Business.Interfaces;
using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ClientsController(IClientService clientService) : ControllerBase
    {
        private readonly IClientService _clientService = clientService;

        // Post
        [HttpPost]
        [UseAdminApiKey]
        public async Task<IActionResult> Create(ClientRegistrationForm form)
        {
            // Validerar mot modellen ClientRegistrationForm
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _clientService.CreateAsync(form);
            return HandleServiceResult(result);
        }

        // Put
        [HttpPut]
        [UseAdminApiKey]
        public async Task<IActionResult> Update(ClientUpdateForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _clientService.UpdateAsync(form);
            return HandleServiceResult(result);
        }

        // Delete
        [HttpDelete("{Id}")]
        [UseAdminApiKey]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _clientService.RemoveAsync(Id);
            return HandleServiceResult(result);
        }

        // Get
        [HttpGet]
        [UseAdminApiKey]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllClientsAsync();

            if (clients == null)
            {
                return NotFound();
            }
            return Ok(clients);
        }

        // Get
        [HttpGet("{id}")]
        [UseAdminApiKey]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        // Get
        [HttpGet("name/{name}")]
        [UseAdminApiKey]
        public async Task<IActionResult> GetClient(string name)
        {
            var client = await _clientService.GetClientByNameAsync(name);

            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        // Sätter till private då den bara behövs som hjälpmetod inom klassen
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