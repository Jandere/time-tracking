using Application.Breaks.Queries;
using Application.Common.Models;

namespace Application.WorkDays.Queries;

public class WorkDayDto : BaseDto<int>
{
    public DateTime Date { get; set; }

    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public string DeveloperId { get; set; } = null!;

    public IList<BreakDto> Breaks { get; set; } = new List<BreakDto>();

    public double TotalWorkedTimeInSeconds { get; set; }
}