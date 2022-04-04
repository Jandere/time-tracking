using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Extensions;

namespace Domain.Entities;

public class WorkDay : BaseEntity<int>
{
    public DateTime Date { get; set; }

    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public string DeveloperId { get; set; } = null!;
    
    [ForeignKey(nameof(DeveloperId))]
    public AppUser? Developer { get; set; }
    
    public IList<Break> Breaks { get; set; } = new List<Break>();

    public double TotalWorkedTimeInSeconds => CalculateWorkedTime();

    private double CalculateWorkedTime()
    {
        if (!EndDate.HasValue)
        {
            var breakStart = Breaks.MinBy(b => b.StartDate);

            switch (breakStart)
            {
                case {EndDate: null}:
                    return breakStart.StartDate.GetDifferenceInSeconds(StartDate);
                case null:
                    return DateTime.Now.GetDifferenceInSeconds(StartDate);
            }
        }
        
        var workedTime = EndDate!.Value.GetDifferenceInSeconds(StartDate);

        var breakTime = Breaks
            .Where(b => b.EndDate.HasValue)
            .Sum(b => b.EndDate!.Value.GetDifferenceInSeconds(b.StartDate));

        return workedTime - breakTime;
    }
}