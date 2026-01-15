using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.Application.CredentialContainers;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public required CredentialContainer CredentialContainer { get; set; }
        public required Guid UserId { get; set; }
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
            Console.WriteLine(request.CredentialContainer.Id);
            var credContainer = await _context.CredentialContainers.FindAsync(request.CredentialContainer.Id);

            if (credContainer == null)
            {
                return Result<Unit>.Failure("Not found");
            }

            if (credContainer.UserId == request.UserId)
            {
                if (credContainer.ContainerHash == request.CredentialContainer.ContainerHash &&
                    credContainer.ContainerString == request.CredentialContainer.ContainerString)
                {
                    return Result<Unit>.Success(Unit.Value);
                }
                credContainer.ContainerHash = request.CredentialContainer.ContainerHash;
                credContainer.ContainerString = request.CredentialContainer.ContainerString;

                try
                {
                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;
                    if (!success)
                    {
                        return Result<Unit>.Failure("Error editing credentials.");
                    }

                    return Result<Unit>.Success(Unit.Value);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine("DbUpdateConcurrency");
                    Console.WriteLine(e);
                    Console.WriteLine(e.StackTrace);
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine("DbUpdate");
                    Console.WriteLine(e);
                    Console.WriteLine(e.StackTrace);
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine("OperationCancelled");
                    Console.WriteLine(e);
                    Console.WriteLine(e.StackTrace);
                }
            }
            return Result<Unit>.Failure("Unauthorized");
        }
    }
}
