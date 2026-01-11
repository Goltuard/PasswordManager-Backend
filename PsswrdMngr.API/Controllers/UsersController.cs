using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PsswrdMngr.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("UsersController OK");
        }
    }
}
