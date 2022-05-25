using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkTasks.Commands.UpdateWorkTask;

public class UpdateWorkTaskCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    
    public DateTime? DeadLine { get; set; }
}

internal class UpdateWorkTaskCommandHandler : IRequestHandler<UpdateWorkTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateWorkTaskCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result> Handle(UpdateWorkTaskCommand request, CancellationToken cancellationToken)
    {
        var workTask = await _context.WorkTasks
            .FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken);

        if (workTask is null)
            return Result.Failure("WorkTask not found");

        _mapper.Map(request, workTask);

        _context.WorkTasks.Update(workTask);
        
        var isSuccess = await _context.SaveChangesAsync(cancellationToken);

        return isSuccess ? Result.Success() : Result.Failure("Error during updating workTask");
    }
}