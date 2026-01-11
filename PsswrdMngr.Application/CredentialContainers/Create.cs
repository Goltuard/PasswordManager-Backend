using MediatR;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class Create
{
    public class Command : IRequest<Unit>
    {
        public required CredentialContainer CredentialContainer { get; set; }
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
            _context.CredentialContainers.Add(request.CredentialContainer);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            return Unit.Value;
        }
    }
}