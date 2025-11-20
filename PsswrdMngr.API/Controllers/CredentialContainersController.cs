using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.API.Controllers
{
    public class CredentialContainersController : BaseApiController
    {
        private readonly DataContext _context;

        public CredentialContainersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet] // api/credentialcontainer   ??
        public async Task<ActionResult<List<CredentialContainer>>> GetContainers()
        {
            return await _context.CredentialContainers.ToListAsync();
        }


        [HttpGet("{id}")] //  api/credentialcontainer/id   ??
        public async Task<ActionResult<CredentialContainer>> GetCredential(Guid id)
        {
            return await _context.CredentialContainers.FindAsync(id);
        }
    }
}
