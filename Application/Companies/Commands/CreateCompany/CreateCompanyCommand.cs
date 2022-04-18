using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Companies.Commands.CreateCompany;

public class CreateCompanyCommand : IRequest<Result>
{
    public string Name { get; set; } = null!;
}

internal class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateCompanyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request);
        company.AdministratorId = _currentUserService.UserId!;

        _context.Companies.Add(company);

        var result = await _context.SaveChangesAsync(cancellationToken);
        
        return result ? Result.Success() : Result.Failure("Error during creating administrator");
    }
}