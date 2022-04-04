using Application.Developers.Queries;

namespace Application.Projects.Queries;

public class ProjectDetailsDto : ProjectDto
{
    public DeveloperDto? TeamLead { get; set; }
    
    public ICollection<DeveloperDto> Developers { get; set; } = new List<DeveloperDto>();
}