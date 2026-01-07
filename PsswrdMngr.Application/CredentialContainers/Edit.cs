using MediatR;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class Edit
{
    public class Command : IRequest<Unit>
    {
        public required CredentialContainer CredentialContainer { get; set; }
        public required Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var credContainer = await _context.CredentialContainers.FindAsync(request.CredentialContainer.Id);

            if (credContainer.UserId == request.UserId)
            {
                credContainer.ContainerHash = request.CredentialContainer.ContainerHash;
                credContainer.ContainerString = request.CredentialContainer.ContainerString;

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
