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
    [Route("api/[controller]")]
    public class CredentialContainersController : ControllerBase
    {
        private readonly DataContext _context;

        public CredentialContainersController(DataContext context)
        {
            _context = context;
        }

        private Guid GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(value);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CredentialContainer>>> GetAll()
        {
            var userId = GetUserId();

            var items = await _context.CredentialContainers
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CredentialContainer>> GetById(Guid id)
        {
            var userId = GetUserId();

            var item = await _context.CredentialContainers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<CredentialContainer>> Create([FromBody] CredentialContainer model)
        {
            var userId = GetUserId();

            if (model.Id == Guid.Empty)
                model.Id = Guid.NewGuid();

            model.UserId = userId;

            _context.CredentialContainers.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CredentialContainer model)
        {
            var userId = GetUserId();

            var existing = await _context.CredentialContainers
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (existing == null) return NotFound();

            existing.ContainerHash = model.ContainerHash;
            existing.ContainerString = model.ContainerString;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            var existing = await _context.CredentialContainers
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (existing == null) return NotFound();

            _context.CredentialContainers.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
