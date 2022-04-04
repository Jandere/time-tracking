using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Queries.GetAllDevelopers;

public class GetAllDevelopersQuery : IRequest<ICollection<DeveloperDto>>
{
    
}

internal class GetAllDevelopersQueryHandler : IRequestHandler<GetAllDevelopersQuery, ICollection<DeveloperDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllDevelopersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<DeveloperDto>> Handle(GetAllDevelopersQuery request, CancellationToken cancellationToken)
    {
        var developers = await _context.AppUsers
            .Where(u => u.RoleName == Role.Developer.Name)
            .ProjectTo<DeveloperDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return developers;
    }
}
