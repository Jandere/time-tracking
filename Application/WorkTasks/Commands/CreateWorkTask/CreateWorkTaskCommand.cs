using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.WorkTasks.Commands.CreateWorkTask;

public class CreateWorkTaskCommand : IRequest<Result>
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    
    public DateTime? DeadLine { get; set; }
    
    public int CompanyId { get; set; }
}

internal class CreateWorkTaskCommandHandler : IRequestHandler<CreateWorkTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkTaskCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(CreateWorkTaskCommand request, CancellationToken cancellationToken)
    {
        var workTask = _mapper.Map<WorkTask>(request);
        workTask.CreatorId = _currentUserService.UserId!;

        _context.WorkTasks.Add(workTask);
        var isSuccess = await _context.SaveChangesAsync(cancellationToken);
        
        return isSuccess ?  Result.Success() : Result.Failure("Error during creating work task");
    }
}