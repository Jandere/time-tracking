using Application.Common.Exceptions;
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
    private readonly ICurrentUserService _currentUserService;

    public GetCompanyDevelopersQueryHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<ICollection<DeveloperDto>> Handle(GetCompanyDevelopersQuery request, CancellationToken cancellationToken)
    {
        if (!await IsValidUser(request.Id, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }
        
        var developers = await _context.AppUsers
            .Where(u => u.RoleName == Role.Developer.Name && u.CompanyId == request.Id)
            .ProjectTo<DeveloperDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return developers;
    }

    private async Task<bool> IsValidUser(int companyId, CancellationToken token)
    {
        if (_currentUserService.UserRoleName == Role.Administrator.Name)
        {
            return await _context.Companies.AnyAsync(
                c => c.AdministratorId == _currentUserService.UserId
                     && c.Id == companyId, token);
        }

        if (_currentUserService.UserRoleName == Role.Developer.Name)
        {
            return await _context.AppUsers.AnyAsync(
                u => u.Id == _currentUserService.UserId
                     && u.CompanyId == companyId, token);
        }

        return false;
    }
}