using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Queries.GetDeveloperById;

public class GetDeveloperByIdQuery : IRequest<DeveloperDto?>
{
    public string Id { get; }

    public GetDeveloperByIdQuery(string id)
    {
        Id = id;
    }
}

internal class GetDeveloperByIdQueryHandler : IRequestHandler<GetDeveloperByIdQuery, DeveloperDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeveloperByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<DeveloperDto?> Handle(GetDeveloperByIdQuery request, CancellationToken cancellationToken)
    {
        var developer = await _context.AppUsers
            .Where(u => u.RoleName == Role.Developer.Name && u.Id == request.Id)
            .ProjectTo<DeveloperDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return developer;
    }
}
