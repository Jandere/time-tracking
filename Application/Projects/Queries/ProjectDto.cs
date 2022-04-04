using Application.Common.Models;

namespace Application.Projects.Queries;

public class ProjectDto : BaseDto<int>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? TeamLeadId { get; set; }
    
    public int CompanyId { get; set; }

    public string TeamLeadFullName { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;
}