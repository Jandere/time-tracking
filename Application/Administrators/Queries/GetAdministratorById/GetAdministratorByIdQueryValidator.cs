using Application.Common.Interfaces;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Administrators.Queries.GetAdministratorById;

public class GetAdministratorByIdQueryValidator : AbstractValidator<GetAdministratorByIdQuery>
{
    public GetAdministratorByIdQueryValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        RuleFor(q => q.Id)
            .Must(id =>
            {
                if (currentUserService.UserRoleName == Role.Main.Name)
                    return true;
                return id == currentUserService.UserId && currentUserService.UserRoleName == Role.Administrator.Name;
            });
    }
}