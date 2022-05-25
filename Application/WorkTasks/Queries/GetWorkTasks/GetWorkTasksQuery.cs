using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkTasks.Queries.GetWorkTasks;

public class GetWorkTasksQuery : IRequest<ICollection<WorkTaskDto>>
{
    
}

internal class GetWorkTasksQueryHandler : IRequestHandler<GetWorkTasksQuery, ICollection<WorkTaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetWorkTasksQueryHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<ICollection<WorkTaskDto>> Handle(GetWorkTasksQuery request, CancellationToken cancellationToken)
    {
        var companyId = await _currentUserService.GetCompanyId();
        var workTasks = await _context.WorkTasks.Where(x => x.CompanyId == companyId)
            .ProjectTo<WorkTaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return workTasks;
    }
}