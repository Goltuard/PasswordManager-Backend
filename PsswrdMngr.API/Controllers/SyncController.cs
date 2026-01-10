using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;
using System.Security.Claims;

namespace PsswrdMngr.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/sync")]
    public class SyncController : ControllerBase
    {
        private readonly DataContext _context;

        public SyncController(DataContext context)
        {
            _context = context;
        }

        private Guid GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(value);
        }

        [HttpGet("containers")]
        public async Task<IActionResult> Pull()
        {
            var userId = GetUserId();

            var containers = await _context.CredentialContainers
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(containers);
        }

        [HttpPost("containers")]
        public async Task<IActionResult> Push([FromBody] List<CredentialContainer> containers)
        {
            var userId = GetUserId();

            foreach (var container in containers)
            {
                container.UserId = userId;
                _context.Update(container);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

