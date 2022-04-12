using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProjectAsTeamLead;

public class UpdateProjectAsTeamLeadCommandValidator : AbstractValidator<UpdateProjectAsTeamLeadCommand>
{
    public UpdateProjectAsTeamLeadCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, token) =>
            {
                return await context.Projects.AnyAsync(project =>
                    project.Id == id && project.TeamLeadId == currentUserService.UserId, token);
            })
            .WithMessage("User must be a team lead of this project");

        RuleFor(command => command.Name)
            .Must(name => !string.IsNullOrEmpty(name))
            .WithMessage("Project name must not be null or empty");
    }
}