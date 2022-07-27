using FluentValidation;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.Contracts.FluentValidators;

public class CreateCredentialRequestValidator : AbstractValidator<CreateCredentialRequest>
{
    public CreateCredentialRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Not valid");

        RuleFor(x => x.AccountUsername)
            .NotEmpty();

        RuleFor(x => x.AccountPassword)
            .NotEmpty();
    }
}
