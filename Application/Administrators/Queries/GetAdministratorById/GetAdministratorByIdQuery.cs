using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Administrators.Queries.GetAdministratorById;

public class GetAdministratorByIdQuery : IRequest<AdministratorDto?>
{
    public GetAdministratorByIdQuery(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

internal class GetAdministratorByIdQueryHandler : IRequestHandler<GetAdministratorByIdQuery, AdministratorDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAdministratorByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<AdministratorDto?> Handle(GetAdministratorByIdQuery request, CancellationToken cancellationToken)
    {
        var administrator = await _context.AppUsers
            .Where(u => u.Id == request.Id && u.RoleName == Role.Administrator.Name)
            .ProjectTo<AdministratorDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return administrator;
    }
}