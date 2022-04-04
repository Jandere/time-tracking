using Application.Common.Interfaces;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Name must not be null or empty");
        
        RuleFor(x => x.TeamLeadId)
            .MustAsync(async (id, token) =>
            {
                var user = await context.AppUsers
                    .FirstOrDefaultAsync(u => u.Id == id, token);

                if (user is null)
                    return false;

                return user.RoleName == Role.Developer.Name;
            }).WithMessage("User is not developer");
    }
}