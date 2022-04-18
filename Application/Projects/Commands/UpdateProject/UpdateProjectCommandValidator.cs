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
        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Name must not be null or empty");
        
        RuleFor(x => x.TeamLeadId)
            .CheckIsCurrentUserIdAsync(currentUserService.UserId)
            .WithMessage("Current user is not team lead");
    }
}