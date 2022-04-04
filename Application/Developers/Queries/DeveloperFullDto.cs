using Application.Projects.Queries;

namespace Application.Developers.Queries;

public class DeveloperFullDto : DeveloperDto
{
    public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
}