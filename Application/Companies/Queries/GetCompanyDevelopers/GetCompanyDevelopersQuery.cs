using Application.Common.Interfaces;
using Application.Developers.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompanyDevelopers;

public class GetCompanyDevelopersQuery : IRequest<ICollection<DeveloperDto>>
{
    public int Id { get; }

    public GetCompanyDevelopersQuery(int id)
    {
        Id = id;
    }
}

internal class GetCompanyDevelopersQueryHandler : IRequestHandler<GetCompanyDevelopersQuery, ICollection<DeveloperDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyDevelopersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<DeveloperDto>> Handle(GetCompanyDevelopersQuery request, CancellationToken cancellationToken)
    {
        var developers = await _context.AppUsers
            .Where(u => u.RoleName == Role.Developer.Name && u.CompanyId == request.Id)
            .ProjectTo<DeveloperDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return developers;
    }
}