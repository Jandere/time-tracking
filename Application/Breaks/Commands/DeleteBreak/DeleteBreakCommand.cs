using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Breaks.Commands.DeleteBreak;

public class DeleteBreakCommand : IRequest<Result>
{
    public int Id { get; }

    public DeleteBreakCommand(int id)
    {
        Id = id;
    }
}

internal class DeleteBreakCommandHandler : IRequestHandler<DeleteBreakCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteBreakCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(DeleteBreakCommand request, CancellationToken cancellationToken)
    {
        var @break = await _context.Breaks
            .Include(b => b.WorkDay)
            .SingleOrDefaultAsync(b => b.Id == request.Id,
                cancellationToken);

        if (@break is null)
            return Result.Failure("Break not found");
        
        _context.Breaks.Remove(@break);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during deleting break");
    }
}