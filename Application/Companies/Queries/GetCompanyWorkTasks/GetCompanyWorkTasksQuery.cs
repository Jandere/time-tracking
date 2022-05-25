using Application.Common.Interfaces;
using Application.WorkTasks.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanyWorkTasks;

public class GetCompanyWorkTasksQuery : IRequest<ICollection<WorkTaskDto>>
{
    public int CompanyId { get; }

    public GetCompanyWorkTasksQuery(int companyId)
    {
        CompanyId = companyId;
    }
}

internal class GetCompanyWorkTasksQueryHandler : IRequestHandler<GetCompanyWorkTasksQuery, ICollection<WorkTaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyWorkTasksQueryHandler(
        IApplicationDbContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<WorkTaskDto>> Handle(GetCompanyWorkTasksQuery request, CancellationToken cancellationToken)
    {
        var workTasks = await _context.WorkTasks
            .Where(workTask => workTask.CompanyId == request.CompanyId)
            .ProjectTo<WorkTaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return workTasks;
    }
}

