using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Administrators.Commands.UpdateAdministrator;

public class UpdateAdministratorCommandValidator : AbstractValidator<UpdateAdministratorCommand>
{
    public UpdateAdministratorCommandValidator(IApplicationDbContext context)
    {
    }
}