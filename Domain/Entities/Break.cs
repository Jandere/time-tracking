using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class Break : BaseEntity<int>
{
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public int WorkDayId { get; set; }

    [ForeignKey(nameof(WorkDayId))]
    public WorkDay WorkDay { get; set; } = null!;
}