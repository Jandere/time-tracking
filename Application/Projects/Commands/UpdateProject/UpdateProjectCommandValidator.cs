using Application.Common.Interfaces;
using Application.Extensions;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, token) =>
            {
                var isProjectExist = await context.Projects
                    .Include(p => p.Company)
                    .AnyAsync(p => p.Company.AdministratorId == currentUserService.UserId && p.Id == id, token);

                return isProjectExist;
            }).WithMessage("Project not found");

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
            }).WithMessage("User is not developer")
            .CheckIsCurrentUserIdAsync(currentUserService.UserId)
            .WithMessage("Current user is not team lead");
    }
}