using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.API.Dto;
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
        public async Task<ActionResult<List<ContainerDto>>> GetContainers()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var results = await Mediator.Send(new List.Query{UserId = userId});
            var dtoResList = new List<ContainerDto>();
            foreach (var res in results)
            {
                dtoResList.Add(new ContainerDto
                {
                    ContainerHash = res.ContainerHash,
                    ContainerString = res.ContainerString,
                    Id = res.Id
                });
            }

            return dtoResList;
        }


        [HttpGet("{id}")] //  api/credentialcontainers/id   ??
        public async Task<ActionResult<ContainerDto>> GetCredential(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await Mediator.Send(new Single.Query { Id = id, UserId = userId});
            var resultDto = new ContainerDto
            {
                ContainerHash = result.ContainerHash,
                ContainerString = result.ContainerString,
                Id = result.Id
            };
            return resultDto;
        }

        [HttpPost] //  api/credentialcontainers
        public async Task<IActionResult> CreateCredentialContainer(ContainerDto containerDto)
        {
            var credentialContainer = new CredentialContainer
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                ContainerHash = containerDto.ContainerHash,
                ContainerString = containerDto.ContainerString
            };
            await Mediator.Send(new Create.Command { CredentialContainer = credentialContainer });
            return Ok();
        }

        [HttpPut("{id}")] //  api/credentialcontainers/id
        public async Task<IActionResult> EditCredentialContainer(Guid id, ContainerDto containerDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var credentialContainer = new CredentialContainer
            {
                Id = id,
                UserId = userId,
                ContainerHash = containerDto.ContainerHash,
                ContainerString = containerDto.ContainerString
            };
            await Mediator.Send(new Edit.Command { CredentialContainer = credentialContainer, UserId = userId});
            return Ok();
        }

        [HttpDelete("{id}")] // api/credentialcontainers/id
        public async Task<IActionResult> RemoveCredentialContainer(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await Mediator.Send(new Delete.Command { Id = id, UserId = userId});
            return Ok();
        }
    }
}
