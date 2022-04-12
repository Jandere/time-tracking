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
            .MustAsync(async (id, token) =>
            {
                if (currentUserService.UserRoleName == Role.Main.Name)
                    return true;

                if (currentUserService.UserRoleName == Role.Developer.Name)
                {
                    return await context.AppUsers.Include(d => d.Company)
                        .AnyAsync(d => d.Company.AdministratorId == id, token);
                }

                return id == currentUserService.UserId && currentUserService.UserRoleName == Role.Administrator.Name;
            });
    }
}