using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkTasks.Commands.FinishWorkTask;

public class FinishWorkTaskCommand : IRequest<Result>
{
    public int Id { get; set; }

    public DateTime FinishDate { get; set; } = DateTime.Now;
}

internal class FinishWorkTaskCommandHandler : IRequestHandler<FinishWorkTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public FinishWorkTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(FinishWorkTaskCommand request, CancellationToken cancellationToken)
    {
        var workTask = await _context.WorkTasks
            .FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken);

        if (workTask is null)
            return Result.Failure("WorkTask not found");
        
        workTask.FinishTime = request.FinishDate;

        var isSuccess = await _context.SaveChangesAsync(cancellationToken);

        return isSuccess ? Result.Success() : Result.Failure("Error during updating workTask");
    }
}