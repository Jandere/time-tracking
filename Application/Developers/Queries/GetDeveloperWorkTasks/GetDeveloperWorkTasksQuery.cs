using Application.Common.Interfaces;
using Application.WorkTasks.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Queries.GetDeveloperWorkTasks;

public class GetDeveloperWorkTasksQuery : IRequest<ICollection<WorkTaskDto>>
{
    public string Id { get; }

    public GetDeveloperWorkTasksQuery(string id)
    {
        Id = id;
    }
}

internal class GetDeveloperWorkTasksQueryHandler : IRequestHandler<GetDeveloperWorkTasksQuery, ICollection<WorkTaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeveloperWorkTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<WorkTaskDto>> Handle(GetDeveloperWorkTasksQuery request, CancellationToken cancellationToken)
    {
        var workTasks = await _context.WorkTaskExecutors
            .Where(x => x.ExecutorId == request.Id)
            .Include(x => x.WorkTask)
            .Select(x => x.WorkTask)
            .ProjectTo<WorkTaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return workTasks;
    }
}