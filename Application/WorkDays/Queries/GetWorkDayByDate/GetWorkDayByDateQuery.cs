using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkDays.Queries.GetWorkDayByDate;

public class GetWorkDayByDateQuery : IRequest<WorkDayDto>
{
    public DateTime Date { get; }

    public GetWorkDayByDateQuery(DateTime date)
    {
        Date = date;
    }
}

internal class GetWorkDayByDateQueryHandler : IRequestHandler<GetWorkDayByDateQuery, WorkDayDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetWorkDayByDateQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<WorkDayDto> Handle(GetWorkDayByDateQuery request, CancellationToken cancellationToken)
    {
        var workDay = await _context.WorkDays
            .ProjectTo<WorkDayDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(w => w.Date.Date == request.Date.Date
                                      && w.DeveloperId == _currentUserService.UserId,
            cancellationToken);

        return workDay ?? WorkDayFactory.GetEmptyWorkDay(request.Date, _currentUserService.UserId!);
    }
}