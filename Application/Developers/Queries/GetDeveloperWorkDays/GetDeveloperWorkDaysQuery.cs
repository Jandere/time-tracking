using Application.Common.Interfaces;
using Application.WorkDays;
using Application.WorkDays.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Queries.GetDeveloperWorkDays;

public class GetDeveloperWorkDaysQuery : IRequest<ICollection<WorkDayDto>>
{
    public string Id { get; }

    public DateTime DateFrom { get; }
    
    public DateTime DateTo { get; }

    public GetDeveloperWorkDaysQuery(string id, DateTime dateFrom, DateTime dateTo)
    {
        Id = id;
        DateFrom = dateFrom;
        DateTo = dateTo;
    }
}

internal class GetDeveloperWorkDaysQueryHandler : IRequestHandler<GetDeveloperWorkDaysQuery, ICollection<WorkDayDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeveloperWorkDaysQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ICollection<WorkDayDto>> Handle(GetDeveloperWorkDaysQuery request, CancellationToken cancellationToken)
    {
        var workDays = await _context.WorkDays
            .Where(x => x.DeveloperId == request.Id && x.Date >= request.DateFrom && x.Date <= request.DateTo)
            .ProjectTo<WorkDayDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return WorkDayFactory.GetWorkDaysRange(request.DateFrom, request.DateTo, workDays);
    }
}