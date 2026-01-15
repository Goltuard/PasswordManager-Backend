using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using MediatR;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public required CredentialContainer CredentialContainer { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CredentialContainer).SetValidator(new CredentialContainerValidator());
        }
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
            var hasher = SHA256.Create();
            var contStringByteArray = Encoding.UTF8.GetBytes(request.CredentialContainer.ContainerString);
            var hash = hasher.ComputeHash(contStringByteArray);
            var hashString = Convert.ToHexString(hash);

            if (request.CredentialContainer.ContainerHash != hashString)
            {
                return Result<Unit>.Failure("Hash validation failed");
            }
            _context.CredentialContainers.Add(request.CredentialContainer);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!success)
            {
                return Result<Unit>.Failure("Error saving credentials.");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}