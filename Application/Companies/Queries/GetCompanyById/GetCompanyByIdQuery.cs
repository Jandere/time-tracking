using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetById;

public class GetCompanyByIdQuery : IRequest<CompanyDetailsDto?>
{
    public int Id { get; }

    public GetCompanyByIdQuery(int id)
    {
        Id = id;
    }
}

internal class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDetailsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetCompanyByIdQueryHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<CompanyDetailsDto?> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = _currentUserService.UserRoleName == Role.Administrator.Name
            ? await GetForAdministrator(request.Id, cancellationToken)
            : await GetForDeveloper(request.Id, cancellationToken);

        return company;
    }

    private async Task<CompanyDetailsDto?> GetForDeveloper(int id, CancellationToken token)
    {
        return await _context.Companies
            .Where(c => c.Id == id && c.Developers.Any(d => d.Id == _currentUserService.UserId))
            .ProjectTo<CompanyDetailsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(token);
    }
    
    private async Task<CompanyDetailsDto?> GetForAdministrator(int id, CancellationToken token)
    {
        return await _context.Companies
            .Where(c => c.Id == id && c.AdministratorId == _currentUserService.UserId)
            .ProjectTo<CompanyDetailsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(token);
    }
}