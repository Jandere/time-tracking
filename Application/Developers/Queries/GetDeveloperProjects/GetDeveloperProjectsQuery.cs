using Application.Common.Interfaces;
using Application.Projects.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Queries.GetDeveloperProjects;

public class GetDeveloperProjectsQuery : IRequest<ICollection<ProjectDto>>
{
    public GetDeveloperProjectsQuery(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

internal class GetDeveloperProjectsQueryHandler : IRequestHandler<GetDeveloperProjectsQuery, ICollection<ProjectDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeveloperProjectsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<ProjectDto>> Handle(GetDeveloperProjectsQuery request, CancellationToken cancellationToken)
    {
        // var projects = await _context.Projects
        //     .Where(x => x.Developers.Select(d => d.DeveloperId).Contains(request.Id))
        //     .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
        //     .ToListAsync(cancellationToken);

        var projects = await _context.DeveloperProjects
            .Where(dp => dp.DeveloperId == request.Id)
            .Select(dp => dp.Project)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return projects;
    }
}