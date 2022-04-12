using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Breaks.Commands.FinishBreak;

public class FinishBreakCommandValidator : AbstractValidator<FinishBreakCommand>
{
    public FinishBreakCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, token) =>
            {
                var isBreakExist = await context.Breaks
                    .Include(b => b.WorkDay)
                    .AnyAsync(b => b.Id == id
                                   && b.WorkDay.DeveloperId == currentUserService.UserId,
                        token);

                return isBreakExist;
            }).WithMessage("Break not found");

        RuleFor(x => x.FinishTime)
            .MustAsync(async (request, finishTime, token) =>
            {
                var @break = await context.Breaks.SingleOrDefaultAsync(b => b.Id == request.Id, token);

                return @break!.StartDate < finishTime;
            })
            .WithMessage("Finish time cannot be less than start time")
            .MustAsync(async (request, finishTime, token) =>
            {
                var @break = await context.Breaks
                    .Include(b => b.WorkDay)
                    .SingleOrDefaultAsync(b => b.Id == request.Id, token);
                
                return @break!.StartDate.Date == finishTime.Date && finishTime.Date == @break.WorkDay.Date.Date;
            })
            .WithMessage("Finish date must be equal to work day date and start time date");
    }   
}