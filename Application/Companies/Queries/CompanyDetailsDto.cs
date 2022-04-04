using Application.Developers.Queries;
using Application.Projects.Queries;

namespace Application.Companies.Queries;

public class CompanyDetailsDto : CompanyDto
{
    public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();

    public ICollection<DeveloperDto> Developers { get; set; } = new List<DeveloperDto>();
}