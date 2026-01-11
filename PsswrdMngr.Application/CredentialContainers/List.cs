using MediatR;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class List
{
    public class Query : IRequest<Result<List<CredentialContainer>>>
    {
        public required Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<CredentialContainer>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<CredentialContainer>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var allContainers = await _context.CredentialContainers.ToListAsync();
            var properContainers = new List<CredentialContainer>();
            
            foreach (var container in allContainers)
            {
                if (container.UserId == request.UserId)
                {
                    properContainers.Add(container);
                }
            }

            return Result<List<CredentialContainer>>.Success(properContainers);
        }
    }
}