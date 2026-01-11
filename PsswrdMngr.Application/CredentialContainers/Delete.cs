using MediatR;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var credContainer = await _context.CredentialContainers.FindAsync(request.Id);

            if (credContainer == null)
            {
                return Result<Unit>.Failure("Not found");
            }
            
            if (credContainer.UserId == request.UserId)
            {
                _context.Remove(credContainer);

                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!success)
                {
                    return Result<Unit>.Failure("Error removing credentials.");
                }

                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure("Unauthorized");
        }
    }
}
