using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Breaks.Commands.AddBreak;

public class AddBreakCommand : IRequest<Result>
{
    public int WorkDayId { get; set; }

    public DateTime StartTime { get; set; }
}

internal class AddBreakCommandHandler : IRequestHandler<AddBreakCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public AddBreakCommandHandler(IApplicationDbContext context, IMapper mapper, 
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(AddBreakCommand request, CancellationToken cancellationToken)
    {
        var workDay = await _context.WorkDays
            .SingleOrDefaultAsync(w => w.Id == request.WorkDayId
                           && w.DeveloperId == _currentUserService.UserId,
                cancellationToken);

        if (workDay is null)
            return Result.Failure("Work day not found");

        if (workDay.Date.Date != request.StartTime.Date || workDay.StartDate?.Date != request.StartTime.Date)
            return Result.Failure("Break and work day has different days");

        if (workDay.StartDate is null || workDay.StartDate > request.StartTime)
            return Result.Failure("Break time must be less than work start time");

        var @break = _mapper.Map<Break>(request);

        _context.Breaks.Add(@break);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during adding break");
    }
}