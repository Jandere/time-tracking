using Application.Projects.Queries;
using Application.WorkDays.Queries;

namespace Application.Developers.Queries;

public class DeveloperFullDto : DeveloperDto
{
    public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();

    public ICollection<WorkDayDto> WorkDays { get; set; } = new List<WorkDayDto>();
}