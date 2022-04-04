using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetAllCompanies;

public class GetAllCompaniesQuery : IRequest<ICollection<CompanyDto>>
{
    
}

internal class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, ICollection<CompanyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllCompaniesQueryHandler(IApplicationDbContext context, IMapper mapper, 
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<ICollection<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _context.Companies
            .Where(c => c.AdministratorId == _currentUserService.UserId)
            .ProjectTo<CompanyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return companies;
    }
}