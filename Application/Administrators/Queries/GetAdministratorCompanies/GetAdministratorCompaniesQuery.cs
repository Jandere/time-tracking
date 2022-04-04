using Application.Common.Interfaces;
using Application.Companies.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Administrators.Queries.GetAdministratorCompanies;

public class GetAdministratorCompaniesQuery : IRequest<ICollection<CompanyDto>>
{
    public GetAdministratorCompaniesQuery(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

internal class GetAdministratorCompaniesQueryHandler : 
    IRequestHandler<GetAdministratorCompaniesQuery, ICollection<CompanyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAdministratorCompaniesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<CompanyDto>> Handle(GetAdministratorCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _context.Companies
            .Where(c => c.AdministratorId == request.Id)
            .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return companies;
    }
}