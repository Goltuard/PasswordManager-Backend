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
            foreach (var res in results.Value)
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
            
            if (result == null || result.Value == null)
            {
                return NotFound();
            }
            
            if (result.IsSuccess)
            {
                var resultDto = new ContainerDto
                {
                    ContainerHash = result.Value.ContainerHash,
                    ContainerString = result.Value.ContainerString,
                    Id = result.Value.Id
                };
                return Ok(resultDto);
            }

            if (result.Error == "Unauthorized")
            {
                // Return 404 Not Found instead of 401 Unauthorized to prevent database enumeration.
                // In case of a correctly structured query the API should return only 2 possible values:
                // 200 OK with the resulting object
                // 404 Not Found in case the credential container doesn't exist or the authenticated user doesn't have access to it
                return NotFound();
            }

            // If the query is incorrect, return 400 Bad Request
            return BadRequest();
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
            var result = await Mediator.Send(new Create.Command { CredentialContainer = credentialContainer });
            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(result.Error);
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
            var result = await Mediator.Send(new Edit.Command { CredentialContainer = credentialContainer, UserId = userId});
            if (result.IsSuccess)
            {
                return Ok();
            }

            if (result.Error == "Not found" || result.Error == "Unauthorized")
            {
                return NotFound();
            }

            return BadRequest(result.Error);
        }

        [HttpDelete("{id}")] // api/credentialcontainers/id
        public async Task<IActionResult> RemoveCredentialContainer(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await Mediator.Send(new Delete.Command { Id = id, UserId = userId});
            if (result.IsSuccess)
            {
                return Ok();
            }
            
            if (result.Error == "Not found" || result.Error == "Unauthorized")
            {
                return NotFound();
            }

            return BadRequest(result.Error);
        }
    }
}
