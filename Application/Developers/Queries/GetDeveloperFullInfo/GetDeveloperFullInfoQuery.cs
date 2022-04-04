using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Queries.GetDeveloperFullInfo;

public class GetDeveloperFullInfoQuery : IRequest<DeveloperFullDto?>
{
    public GetDeveloperFullInfoQuery(string id)
    {
        Id = id;
    }

    public string Id { get; }
}

internal class GetDeveloperFullInfoQueryHandler : IRequestHandler<GetDeveloperFullInfoQuery, DeveloperFullDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeveloperFullInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<DeveloperFullDto?> Handle(GetDeveloperFullInfoQuery request, CancellationToken cancellationToken)
    {
        var developer = await _context.AppUsers
            .Where(u => u.RoleName == Role.Developer.Name && u.Id == request.Id)
            .ProjectTo<DeveloperFullDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return developer;
    }
}