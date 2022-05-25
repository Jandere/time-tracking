using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkTasks.Queries.GetWorkTask;

public class GetWorkTaskQuery : IRequest<WorkTaskDto?>
{
    public int Id { get; }

    public GetWorkTaskQuery(int id)
    {
        Id = id;
    }
}

internal class GetWorkTaskQueryHandler : IRequestHandler<GetWorkTaskQuery, WorkTaskDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkTaskQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<WorkTaskDto?> Handle(GetWorkTaskQuery request, CancellationToken cancellationToken)
    {
        var workTask = await _context.WorkTasks
            .FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken);

        var dto = _mapper.Map<WorkTaskDto>(workTask);
        
        return dto;
    }
}