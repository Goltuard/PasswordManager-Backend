using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Application.CredentialContainers;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.API.Controllers
{
    public class CredentialContainersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CredentialContainersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet] // api/credentialcontainer   ??
        public async Task<ActionResult<List<CredentialContainer>>> GetContainers()
        {
            return await _mediator.Send(new List.Query());
        }


        [HttpGet("{id}")] //  api/credentialcontainer/id   ??
        public async Task<ActionResult<CredentialContainer>> GetCredential(Guid id)
        {
            return await _context.CredentialContainers.FindAsync(id);
        }
    }
}
