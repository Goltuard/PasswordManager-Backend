using FluentValidation;
using PsswrdMngr.Domain;

namespace PsswrdMngr.Application;

public class CredentialContainerValidator : AbstractValidator<CredentialContainer>
{
    public CredentialContainerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Container ID missing");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Container owner ID missing");
        RuleFor(x => x.ContainerHash).NotEmpty().WithMessage("Container hash missing");
        RuleFor(x => x.ContainerString).NotEmpty().WithMessage("Container string missing");
    }
    
}