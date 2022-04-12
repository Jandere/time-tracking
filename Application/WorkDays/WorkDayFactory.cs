using Application.WorkDays.Queries;
using Domain.Extensions;

namespace Application.WorkDays;

public static class WorkDayFactory
{
    public static ICollection<WorkDayDto> GetWorkDaysRange(DateTime dateFrom, DateTime dateTo,
        ICollection<WorkDayDto> exist)
    {
        var dates = dateFrom.GetDatesUntil(dateTo);
        return GetWorkDaysRange(dates, exist);
    }

    public static ICollection<WorkDayDto> GetWorkDaysRange(ICollection<DateTime> dates, ICollection<WorkDayDto> exist)
    {
        return dates.Select(date =>
                exist.FirstOrDefault(e => e.Date.Date == date.Date) ??
                GetEmptyWorkDay(date.Date))
            .ToList();
    }

    public static WorkDayDto GetEmptyWorkDay(DateTime date)
    {
        return GetEmptyWorkDay(date, string.Empty);
    }

    public static WorkDayDto GetEmptyWorkDay(DateTime date, string userId)
    {
        return new WorkDayDto
        {
            Date = date,
            StartDate = null,
            EndDate = null,
            DeveloperId = userId,
            TotalWorkedTimeInSeconds = 0
        };
    }
}