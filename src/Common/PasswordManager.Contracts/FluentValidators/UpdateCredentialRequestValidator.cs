using FluentValidation;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.Contracts.FluentValidators;

public class UpdateCredentialRequestValidator : AbstractValidator<UpdateCredentialRequest>
{
    public UpdateCredentialRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.AccountUsername)
            .NotEmpty();

        RuleFor(x => x.AccountPassword)
            .NotEmpty();
    }
}
