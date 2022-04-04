using Application.Common.Interfaces;
using Application.Projects.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanyProjects;

public class GetCompanyProjectsQuery : IRequest<ICollection<ProjectDto>>
{
    public int Id { get; }

    public GetCompanyProjectsQuery(int id)
    {
        Id = id;
    }
}

internal class GetCompanyProjectQueryHandler : IRequestHandler<GetCompanyProjectsQuery, ICollection<ProjectDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyProjectQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<ProjectDto>> Handle(GetCompanyProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .Where(p => p.CompanyId == request.Id)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return projects;
    }
}