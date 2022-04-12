using Application.Breaks.Queries;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkDays.Queries.GetBreaks;

public class GetBreaksQuery : IRequest<ICollection<BreakDto>>
{
    public GetBreaksQuery(int id)
    {
        Id = id;
    }

    public int Id { get; }
}

internal class GetBreaksQueryHandler : IRequestHandler<GetBreaksQuery, ICollection<BreakDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetBreaksQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<ICollection<BreakDto>> Handle(GetBreaksQuery request, CancellationToken cancellationToken)
    {
        ICollection<BreakDto> breaks = _currentUserService.UserRoleName switch
        {
            nameof(Role.Developer) => await _context.Breaks
                .Where(b => b.WorkDayId == request.Id && b.WorkDay.DeveloperId == _currentUserService.UserId)
                .Include(b => b.WorkDay)
                .ProjectTo<BreakDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken),
            nameof(Role.Administrator) => await _context.Breaks.Where(b => b.WorkDayId == request.Id)
                .Include(b => b.WorkDay)
                .ProjectTo<BreakDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken),
            _ => throw new BusinessLogicException("User has no access")
        };

        return breaks;
    }
}