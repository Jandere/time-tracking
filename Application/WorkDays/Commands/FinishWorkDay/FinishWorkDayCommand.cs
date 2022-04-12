using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkDays.Commands.FinishWorkDay;

public class FinishWorkDayCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public DateTime FinishTime { get; set; }
}

internal class FinishWorkDayCommandHandler : IRequestHandler<FinishWorkDayCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public FinishWorkDayCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(FinishWorkDayCommand request, CancellationToken cancellationToken)
    {
        var workDay = await _context.WorkDays
            .FirstOrDefaultAsync(w => w.DeveloperId == _currentUserService.UserId
                                      && w.Id == request.Id 
                                      && w.Date.Date == request.FinishTime.Date,
                cancellationToken);

        if (workDay is null)
            return Result.Failure("WorkDay not found");
        
        workDay.EndDate = request.FinishTime;

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during updating work day");
    }
}