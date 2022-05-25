using Application.Common.Models;
using Application.Developers.Queries;

namespace Application.WorkTasks.Queries;

public class WorkTaskDto : BaseDto<int>
{
    public string CreatorId { get; set; } = null!;

    public string CreatorName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    
    public DateTime? DeadLine { get; set; }

    public bool IsFinished { get; set; }
    
    public DateTime FinishTime { get; set; }
    
    public int CompanyId { get; set; }
    
    public ICollection<DeveloperDto> Executors { get; set; } = new List<DeveloperDto>();
}