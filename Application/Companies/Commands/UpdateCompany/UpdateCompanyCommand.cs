using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string? ImgPath { get; set; }

    public string? Description { get; set; }
    
    public string? Address { get; set; }

    public string? Bin { get; set; }
}

internal class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCompanyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .SingleOrDefaultAsync(c => request.Id == c.Id, cancellationToken);

        if (company is null)
            return Result.Failure("Company not found");
        
        if (company.AdministratorId != _currentUserService.UserId)
            return Result.Failure("Company administrator is not current user");

        company.Name = request.Name;

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during updating company");
    }
}