using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Application.CredentialContainers;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;
using Single = PsswrdMngr.Application.CredentialContainers.Single;

namespace PsswrdMngr.API.Controllers
{
    public class CredentialContainersController : BaseApiController
    {
        public CredentialContainersController()
        { }

        [HttpGet] // api/credentialcontainers   ??
        public async Task<ActionResult<List<CredentialContainer>>> GetContainers()
        {
            return await Mediator.Send(new List.Query());
        }


        [HttpGet("{id}")] //  api/credentialcontainers/id   ??
        public async Task<ActionResult<CredentialContainer>> GetCredential(Guid id)
        {
            return await Mediator.Send(new Single.Query { Id = id });
        }

        [HttpPost] //  api/credentialcontainers
        public async Task<IActionResult> CreateCredentialContainer(CredentialContainer credentialContainer)
        {
            await Mediator.Send(new Create.Command { CredentialContainer = credentialContainer });
            return Ok();
        }

        [HttpPut("{id}")] //  api/credentialcontainers/id
        public async Task<IActionResult> EditCredentialContainer(Guid id, CredentialContainer credentialContainer)
        {
            credentialContainer.Id = id;
            await Mediator.Send(new Edit.Command { CredentialContainer = credentialContainer });
            return Ok();
        }

        [HttpDelete("{id}")] // api/credentialcontainers/id
        public async Task<IActionResult> RemoveCredentialContainer(Guid id)
        {
            await Mediator.Send(new Delete.Command { Id = id });
            return Ok();
        }
    }
}
