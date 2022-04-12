using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Breaks.Commands.AddBreak;

public class AddBreakCommandValidator : AbstractValidator<AddBreakCommand>
{
    public AddBreakCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        RuleFor(x => x.WorkDayId)
            .MustAsync(async (workDayId, token) =>
            {
                var isWorkDayExist = await context.WorkDays
                    .AnyAsync(w => w.Id == workDayId
                                               && w.DeveloperId == currentUserService.UserId,
                        token);

                return isWorkDayExist;
            }).WithMessage("WorkDay not found");

        RuleFor(x => x.StartTime)
            .MustAsync(async (model, startTime, token) =>
            {
                var workDay = await context.WorkDays
                    .SingleOrDefaultAsync(w => w.Id == model.WorkDayId
                                               && w.DeveloperId == currentUserService.UserId, token);

                return startTime.Date == workDay!.Date;
            }).WithMessage("Start date must be equal to work day date");
    }
}