using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Administrators.Queries.GetAllAdministrators;

public class GetAllAdministratorsQuery : IRequest<ICollection<AdministratorDto>>
{
    
}

internal class GetAllAdministratorsQueryHandler : IRequestHandler<GetAllAdministratorsQuery, ICollection<AdministratorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAdministratorsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<AdministratorDto>> Handle(GetAllAdministratorsQuery request, CancellationToken cancellationToken)
    {
        var administrators = await _context.AppUsers
            .Where(x => x.RoleName == Role.Administrator.Name)
            .ProjectTo<AdministratorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return administrators;
    }
}