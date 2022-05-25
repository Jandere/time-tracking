using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkTasks.Commands.DeleteWorkTask;

public class DeleteWorkTaskCommand : IRequest<Result>
{
    public int Id { get; }

    public DeleteWorkTaskCommand(int id)
    {
        Id = id;
    }
}

internal class DeleteWorkTaskCommandHandler : IRequestHandler<DeleteWorkTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(DeleteWorkTaskCommand request, CancellationToken cancellationToken)
    {
        var workTask = await _context.WorkTasks
            .FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken);

        if (workTask is null)
            return Result.Failure("WorkTask not found");

        _context.WorkTasks.Remove(workTask);
        
        var isSuccess = await _context.SaveChangesAsync(cancellationToken);

        return isSuccess ? Result.Success() : Result.Failure("Error during deleting workTask");
    }
}