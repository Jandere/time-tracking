using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Breaks.Commands.FinishBreak;

public class FinishBreakCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public DateTime FinishTime { get; set; }
}

internal class FinishBreakCommandHandler : IRequestHandler<FinishBreakCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public FinishBreakCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(FinishBreakCommand request, CancellationToken cancellationToken)
    {
        var @break = await _context.Breaks
            .Include(b => b.WorkDay)
            .SingleOrDefaultAsync(b => b.Id == request.Id 
                                       && b.WorkDay.DeveloperId == _currentUserService.UserId, 
                cancellationToken);

        if (@break is null)
            return Result.Failure("Break not found");

        if (@break.StartDate > request.FinishTime)
            return Result.Failure("Finish time can't be less than start time");
        
        if (@break.StartDate.Date != request.FinishTime.Date || @break.WorkDay.Date.Date != request.FinishTime.Date)
            return Result.Failure("Finish date can't be not equal to start date or work day date");
        
        @break.EndDate = request.FinishTime;

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during finishing break");
    }
}