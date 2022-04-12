using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

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

    public AddBreakCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result> Handle(AddBreakCommand request, CancellationToken cancellationToken)
    {
        var @break = _mapper.Map<Break>(request);

        _context.Breaks.Add(@break);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during adding break");
    }
}