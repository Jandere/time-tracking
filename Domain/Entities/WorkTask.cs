using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class WorkTask : BaseEntity<int>
{
    public string CreatorId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    
    public DateTime? DeadLine { get; set; }

    public bool IsFinished { get; set; }
    
    public DateTime FinishTime { get; set; }

    public int CompanyId { get; set; }
    
    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }

    public IList<WorkTaskExecutor> Executors { get; set; } = new List<WorkTaskExecutor>();
}