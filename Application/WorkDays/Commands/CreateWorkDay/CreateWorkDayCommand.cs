using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.WorkDays.Commands.CreateWorkDay;

public class CreateWorkDayCommand : IRequest<Result>
{
    public DateTime Date { get; set; }
    
    public DateTime? StartDate { get; set; }
}

internal class CreateWorkDayCommandHandler : IRequestHandler<CreateWorkDayCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkDayCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(CreateWorkDayCommand request, CancellationToken cancellationToken)
    {
        var workDay = _mapper.Map<WorkDay>(request);

        workDay.DeveloperId = _currentUserService.UserId!;

        _context.WorkDays.Add(workDay);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during creating work day");
    }
}