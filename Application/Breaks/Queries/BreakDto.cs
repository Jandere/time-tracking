using Application.Common.Models;

namespace Application.Breaks.Queries;

public class BreakDto : BaseDto<int>
{
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public int WorkDayId { get; set; }
}