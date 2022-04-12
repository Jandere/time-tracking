using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkDays.Commands.CreateWorkDay;

public class CreateWorkDayCommandValidator : AbstractValidator<CreateWorkDayCommand>
{
    public CreateWorkDayCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        RuleFor(x => x.Date)
            .MustAsync(async (date, token) => !(await context.WorkDays.AnyAsync(w =>
                w.DeveloperId == currentUserService.UserId! && w.Date.Date == date.Date, token)))
            .WithMessage("This day already exist");
    }
}