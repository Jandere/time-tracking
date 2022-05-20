using Application.Common.Interfaces;
using Application.WorkDays.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.WorkDays.Commands.GetOrCreateWorkDay;

public class GetOrCreateWorkDayCommand : IRequest<WorkDayDto>
{
    public GetOrCreateWorkDayCommand(DateTime date)
    {
        Date = date;
    }

    public DateTime Date { get; set; }
}

internal class GetOrCreateWorkDayCommandHandler : IRequestHandler<GetOrCreateWorkDayCommand, WorkDayDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetOrCreateWorkDayCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<WorkDayDto> Handle(GetOrCreateWorkDayCommand request, CancellationToken cancellationToken)
    {
        var workDay = await _context.WorkDays
            .Where(workDay => workDay.Date.Date == request.Date.Date)
            .ProjectTo<WorkDayDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (workDay is not null)
            return workDay;

        var workDayToCreate = new WorkDay
        {
            Date = request.Date.Date,
            StartDate = request.Date,
            DeveloperId = _currentUserService.UserId!
        };

        _context.WorkDays.Add(workDayToCreate);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkDayDto>(workDayToCreate);
    }
}