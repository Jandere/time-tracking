using Hangfire;
using Infrastructure.Services;

namespace Infrastructure;

public static class HangFireJobScheduler
{
    public static void SetJobs()
    {
        RecurringJob.AddOrUpdate<EndWorkDayService>(
            nameof(EndWorkDayService),
            x => x.EndWorkDays(),
            Cron.Minutely);
    }
}