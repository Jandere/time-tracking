using Application.Common.Interfaces;
using Domain.Enums;
using FluentValidation;

namespace Application.Administrators.Queries.GetAdministratorCompanies;

public class GetAdministratorCompaniesQueryValidator : AbstractValidator<GetAdministratorCompaniesQuery>
{
    public GetAdministratorCompaniesQueryValidator(ICurrentUserService currentUserService)
    {
        RuleFor(q => q.Id)
            .Must(id =>
            {
                if (currentUserService.UserRoleName == Role.Main.Name)
                    return true;
                
                return id == currentUserService.UserId && currentUserService.UserRoleName == Role.Administrator.Name;
            })
            .WithMessage("Current user is not requested administrator");
    }
}