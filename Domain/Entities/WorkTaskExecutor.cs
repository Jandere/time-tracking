using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class WorkTaskExecutor : BaseEntity<int>
{
    public int WorkTaskId { get; set; }

    public string ExecutorId { get; set; } = null!;

    [ForeignKey(nameof(WorkTaskId))]
    public WorkTask? WorkTask { get; set; }
    
    [ForeignKey(nameof(ExecutorId))]
    public AppUser? Executor { get; set; }
}