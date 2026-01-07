using MediatR;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class Single
{
    public class Query : IRequest<CredentialContainer>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, CredentialContainer>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<CredentialContainer> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.CredentialContainers.FindAsync(request.Id);
        }
    }
}
