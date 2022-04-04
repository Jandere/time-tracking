using Application.Common.Models;

namespace Application.Companies.Queries;

public class CompanyDto : BaseDto<int>
{
    public string Name { get; set; } = string.Empty;

    public string AdministratorId { get; set; } = null!;
}