using Application.Common.Interfaces;
using Domain.Enums;
using FluentValidation;

namespace Application.Developers.Queries.GetDeveloperWorkDays;

public class GetDeveloperWorkDaysQueryValidator : AbstractValidator<GetDeveloperWorkDaysQuery>
{
    public GetDeveloperWorkDaysQueryValidator(ICurrentUserService currentUserService)
    {
        RuleFor(q => q.Id)
            .Must(id =>
            {
                if (currentUserService.UserRoleName == Role.Administrator.Name)
                    return true;
                
                return id == currentUserService.UserId;
            })
            .WithMessage("Current user is not requested developer");

        RuleFor(q => q.DateFrom)
            .Must((model, dateFrom) => dateFrom < model.DateTo)
            .WithMessage("Date from must be less than date to");
    }
}