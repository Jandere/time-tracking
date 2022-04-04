using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommand : IRequest<Result>
{
    public int Id { get; }

    public DeleteCompanyCommand(int id)
    {
        Id = id;
    }
}

internal class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCompanyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .SingleOrDefaultAsync(c => request.Id == c.Id, cancellationToken);

        if (company is null)
            return Result.Failure("Company not found");
        
        if (company.AdministratorId != _currentUserService.UserId)
            return Result.Failure("Company administrator is not current user");

        _context.Companies.Remove(company);
        
        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during deleting company");
    }
}