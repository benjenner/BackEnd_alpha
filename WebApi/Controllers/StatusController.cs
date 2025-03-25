using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController(StatusService statusService) : ControllerBase
    {
        private readonly StatusService _statusService = statusService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _statusService.GetProjectStatuses();
            return Ok(statuses);
        }
    }
}