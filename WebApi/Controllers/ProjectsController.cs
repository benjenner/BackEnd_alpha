using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    // Detta är sökvägen "/api/projects"(den tar bort controller från namnet på klassen)
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        // Här läggs olika endpoints (GET,PUT osv). CRUD-funktionalitet

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    // Statuskod 200
        //    return Ok();

        //    // Statuskod 400
        //    return BadRequest();

        //    //
        //}

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    // Statuskod 200
        //    return Ok();

        //    // Statuskod 400
        //    return BadRequest();

        //    //
        //}
    }
}