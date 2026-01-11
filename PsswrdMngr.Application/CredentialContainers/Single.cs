using System.Diagnostics.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class Single
{
    public class Query : IRequest<Result<CredentialContainer>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<CredentialContainer>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<CredentialContainer>> Handle(Query request, CancellationToken cancellationToken)
        {
            var credContainer = await _context.CredentialContainers.FindAsync(request.Id);

            if (credContainer.UserId == request.UserId)
            {
                return Result<CredentialContainer>.Success(credContainer);
            }

            return Result<CredentialContainer>.Failure("Unauthorized");
        }
    }
}
