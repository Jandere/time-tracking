using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProjectAsTeamLead;

public class UpdateProjectAsTeamLeadCommandValidator : AbstractValidator<UpdateProjectAsTeamLeadCommand>
{
    public UpdateProjectAsTeamLeadCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        RuleFor(command => command.Name)
            .Must(name => !string.IsNullOrEmpty(name))
            .WithMessage("Project name must not be null or empty");
    }
}