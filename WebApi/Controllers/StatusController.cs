using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class StatusController(IStatusService statusService) : ControllerBase
    {
        private readonly IStatusService _statusService = statusService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _statusService.GetProjectStatuses();
            return Ok(statuses);
        }
    }
}