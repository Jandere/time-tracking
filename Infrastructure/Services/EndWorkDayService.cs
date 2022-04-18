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
    
    [AutomaticRetry(Attempts = 0)]
    public async Task EndWorkDays()
    {
        var now = DateTime.Now;

        var workDaysWorked8HoursFilter = 
            (WorkDay w) => now - w.StartDate!.Value > TimeSpan.FromHours(8) && now.Date == w.StartDate.Value.Date;

        var workDaysFinishedDayFilter =
            (WorkDay w) => now.Date != w.StartDate!.Value.Date;
        
        var workDays = await _context.WorkDays
            .Where(x => x.Date.Date >= now.AddDays(-5))
            .Where(x => x.StartDate != null)
            .Where(w => now - w.StartDate!.Value > TimeSpan.FromHours(8) && now.Date == w.StartDate.Value.Date
                        || now.Date != w.StartDate!.Value.Date)
            .ToListAsync();

        var workDaysWorked8Hours = workDays.Where(workDaysWorked8HoursFilter).ToList();

        var workDaysFinishedDay = workDays.Where(workDaysFinishedDayFilter).ToList();
        
        // workDaysWorked8Hours.ForEach(x => x.EndDate = now);
        // workDaysFinishedDay.ForEach(x => x.EndDate = x.StartDate!.Value.Date.AddDays(1).AddSeconds(-1));

        Parallel.ForEach(workDaysWorked8Hours, x => x.EndDate = now);
        Parallel.ForEach(workDaysFinishedDay, x => x.EndDate = x.StartDate!.Value.Date.AddDays(1).AddSeconds(-1));

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}