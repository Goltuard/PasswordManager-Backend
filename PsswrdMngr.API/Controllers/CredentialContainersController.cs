using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class CredentialContainersController : ControllerBase
    {
        private readonly DataContext _context;

        public CredentialContainersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CredentialContainer>>> GetAll()
        {
            var items = await _context.CredentialContainers
                .AsNoTracking()
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CredentialContainer>> GetById(Guid id)
        {
            var item = await _context.CredentialContainers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<CredentialContainer>> Create([FromBody] CredentialContainer model)
        {
            if (model.Id == Guid.Empty)
                model.Id = Guid.NewGuid();

            _context.CredentialContainers.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CredentialContainer model)
        {
            var existing = await _context.CredentialContainers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null) return NotFound();

            existing.UserId = model.UserId;
            existing.ContainerHash = model.ContainerHash;
            existing.ContainerString = model.ContainerString;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _context.CredentialContainers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null) return NotFound();

            _context.CredentialContainers.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
