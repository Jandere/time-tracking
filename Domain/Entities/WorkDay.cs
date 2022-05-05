using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Extensions;

namespace Domain.Entities;

public class WorkDay : BaseEntity<int>
{
    public DateTime Date { get; set; }

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public string DeveloperId { get; set; } = null!;
    
    [ForeignKey(nameof(DeveloperId))]
    public AppUser? Developer { get; set; }
    
    public IList<Break> Breaks { get; set; } = new List<Break>();

    public double TotalWorkedTimeInSeconds => CalculateWorkedTime();

    private double CalculateWorkedTime()
    {
        if (!StartDate.HasValue)
            return 0.0;
        
        if (!EndDate.HasValue)
        {
            var workedTime2 = DateTime.Now.GetDifferenceInSeconds(StartDate.Value);
            var breakTime2 = Breaks
                .Where(b => b.EndDate.HasValue)
                .Sum(b => b.EndDate!.Value.GetDifferenceInSeconds(b.StartDate));
            return workedTime2 - breakTime2;
        }
        
        var workedTime = EndDate!.Value.GetDifferenceInSeconds(StartDate.Value);

        var breakTime = Breaks
            .Where(b => b.EndDate.HasValue)
            .Sum(b => b.EndDate!.Value.GetDifferenceInSeconds(b.StartDate));

        return workedTime - breakTime;
    }
}