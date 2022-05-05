using Application.Common.Interfaces;
using Domain.Entities;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class EndWorkDayService
{
    private readonly IApplicationDbContext _context;

    public EndWorkDayService(IApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task<List<WorkDay>> GetWorkDays(DateTime date)
    {
        return await _context.WorkDays
            .Where(x => x.Date.Date >= date.AddDays(-1))
            .Where(x => x.StartDate != null)
            .Where(x => x.EndDate == null)
            .Where(w => date.Date != w.StartDate!.Value.Date)
            .ToListAsync();
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task EndWorkDays()
    {
        var now = DateTime.Now;
        var workDays = await GetWorkDays(now);

        Parallel.ForEach(workDays, SetWorkDayFinishTime);

        await _context.SaveChangesAsync(CancellationToken.None);
    }

    private static void SetWorkDayFinishTime(WorkDay workDay)
    {
        var date = workDay.StartDate!.Value.Date;

        var breakTimeInSeconds = workDay.Breaks.Sum(x => (x.EndDate - x.StartDate)?.TotalSeconds ?? 0);

        var breakTime = TimeSpan.FromSeconds(breakTimeInSeconds);
        
        var finishTime = date.AddHours(8).Add(breakTime);

        if (finishTime.Date != date.Date)
        {
            finishTime = date.Date.AddDays(1).AddSeconds(-1);
        }

        workDay.EndDate = finishTime;
    }
}