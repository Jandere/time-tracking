using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Administrators.Commands.DeleteAdministrator;

public class DeleteAdministratorCommandValidator : AbstractValidator<DeleteAdministratorCommand>
{
    public DeleteAdministratorCommandValidator(ICurrentUserService currentUserService)
    {
        RuleFor(c => c.Id)
            .Must(id => currentUserService.UserId == id)
            .WithMessage("Current user is not requested user");
    }
}