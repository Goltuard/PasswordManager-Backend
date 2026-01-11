using MediatR;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class List
{
    public class Query : IRequest<List<CredentialContainer>> {}

    public class Handler : IRequestHandler<Query, List<CredentialContainer>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CredentialContainer>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.CredentialContainers.ToListAsync();
        }
    }
}