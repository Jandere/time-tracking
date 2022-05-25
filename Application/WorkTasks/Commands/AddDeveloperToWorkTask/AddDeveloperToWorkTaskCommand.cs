using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkTasks.Commands.AddDeveloperToWorkTask;

public class AddDeveloperToWorkTaskCommand : IRequest<Result>
{
    public int Id { get; set; }

    public string DeveloperId { get; set; } = null!;
}

internal class AddDeveloperToWorkTaskCommandHandler : IRequestHandler<AddDeveloperToWorkTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public AddDeveloperToWorkTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(AddDeveloperToWorkTaskCommand request, CancellationToken cancellationToken)
    {
        var workTaskExist = await _context.WorkTasks
            .AnyAsync(x => x.Id == request.Id,
                cancellationToken);

        if (!workTaskExist)
            return Result.Failure("WorkTask not found");

        var developerExist = await _context.AppUsers
            .AnyAsync(x => x.Id == request.DeveloperId && x.RoleName == Role.Developer.Name,
                cancellationToken);
        
        if (!developerExist)
            return Result.Failure("Developer not found");

        var workTaskExecutor = new WorkTaskExecutor
        {
            ExecutorId = request.DeveloperId,
            WorkTaskId = request.Id
        };

        _context.WorkTaskExecutors.Add(workTaskExecutor);
        
        var isSuccess = await _context.SaveChangesAsync(cancellationToken);

        return isSuccess ? Result.Success() : Result.Failure("Error during attaching developer to workTask");
    }
}