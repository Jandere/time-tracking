using Application.Common.Interfaces;
using Domain.Enums;
using FluentValidation;

namespace Application.Administrators.Commands.UpdateAdministrator;

public class UpdateAdministratorCommandValidator : AbstractValidator<UpdateAdministratorCommand>
{
    public UpdateAdministratorCommandValidator(ICurrentUserService currentUserService)
    {
        RuleFor(c => c.Id)
            .Must(id => currentUserService.UserId == id)
            .WithMessage("Current user is not requested administrator")
            .Must(_ => currentUserService.UserRoleName == Role.Administrator.Name)
            .WithMessage("Current user is not administrator");

        RuleFor(c => c.UserName)
            .NotEmpty()
            .WithMessage("Username must not me empty")
            .NotNull()
            .WithMessage("Username must not be null")
            .Length(1, 20)
            .WithMessage("Username must be longer than 1 char and shorter than 20 chars");
    }
}