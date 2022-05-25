using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkTasks.Commands.RemoveDeveloperToWorkTask;

public class RemoveDeveloperFromWorkTaskCommand : IRequest<Result>
{
    public int Id { get; set; }

    public string DeveloperId { get; set; } = null!;
}

internal class RemoveDeveloperFromWorkTaskCommandHandler : IRequestHandler<RemoveDeveloperFromWorkTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public RemoveDeveloperFromWorkTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(RemoveDeveloperFromWorkTaskCommand request, CancellationToken cancellationToken)
    {
        var workTaskExecutor = await _context.WorkTaskExecutors
            .FirstOrDefaultAsync(x => x.WorkTaskId == request.Id
                                      && x.ExecutorId == request.DeveloperId,
                cancellationToken);

        if (workTaskExecutor is null)
            return Result.Failure("WorkTaskExecutor not found");

        _context.WorkTaskExecutors.Remove(workTaskExecutor);
        
        var isSuccess = await _context.SaveChangesAsync(cancellationToken);

        return isSuccess ? Result.Success() : Result.Failure("Error during deleting work task executor");
    }
}