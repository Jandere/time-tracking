using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Administrators.Commands.CreateAdministrator;

public class CreateAdministratorCommandValidator : AbstractValidator<CreateAdministratorCommand>
{
    public CreateAdministratorCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .WithMessage("Username must not be null")
            .NotEmpty()
            .WithMessage("Username must not be empty");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Password must not be null")
            .NotEmpty()
            .WithMessage("Password must not be empty")
            .MinimumLength(4)
            .WithMessage("Password must have minimum 4 symbols")
            .Must(password => password.Any(char.IsUpper))
            .WithMessage("Password must have at least one upper symbol")
            .Must(password => password.Any(char.IsLower))
            .WithMessage("Password must have at least one lower symbol");
    }
}