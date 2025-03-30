using Business.Interfaces;
using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    // Authorize kräver att
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(IProjectService projectService) : ControllerBase
    {
        //   "[Authorize (Roles = "Admin")] " kan också sättas på separata metoder.

        //   Om "[Authorize (Roles = "Admin")]" är satt på hela controllern
        //   kan " [AllowAnonyoumus] " tillåta att en separat metod eller anrop ändå kan visas
        //   trots att AppUser inte har admin som roll.

        private readonly IProjectService _projectService = projectService;

        // Post
        [HttpPost]
        public async Task<IActionResult> Create(ProjectRegistrationForm form)
        {
            // Validerar mot modellen ClientRegistrationForm
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _projectService.CreateAsync(form);
            return HandleServiceResult(result);
        }

        // Put
        [HttpPut]
        public async Task<IActionResult> Update(ProjectUpdateForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _projectService.UpdateAsync(form);
            return HandleServiceResult(result);
        }

        // Delete
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _projectService.RemoveAsync(Id);
            return HandleServiceResult(result);
        }

        // Get
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetProjectsAsync();

            if (projects == null)
            {
                return NotFound();
            }
            return Ok(projects);
        }

        // Get
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        // Get
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetProject(string name)
        {
            var project = await _projectService.GetProjectByNameAsync(name);

            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
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