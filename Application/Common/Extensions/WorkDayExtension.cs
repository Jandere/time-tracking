using Domain.Entities;
using Domain.Extensions;

namespace Application.Common.Extensions;

public static class WorkDayExtension
{
    public static double CalculateWorkTime(this WorkDay workDay, DateTime? now = null)
    {
        now ??= DateTime.Now;

        if (workDay.StartDate is null)
            return 0.0;
        
        if (!workDay.EndDate.HasValue)
        {
            var breakStart = workDay.Breaks.MinBy(b => b.StartDate);

            switch (breakStart)
            {
                case {EndDate: null}:
                    return breakStart.StartDate.GetDifferenceInSeconds(workDay.StartDate.Value);
                case null:
                    return now.Value.GetDifferenceInSeconds(workDay.StartDate.Value);
            }
        }
        
        var workedTime = workDay.EndDate!.Value.GetDifferenceInSeconds(workDay.StartDate.Value);

        var breakTime = workDay.Breaks
            .Where(b => b.EndDate.HasValue)
            .Sum(b => b.EndDate!.Value.GetDifferenceInSeconds(b.StartDate));

        return workedTime - breakTime;
    }
}