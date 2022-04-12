namespace Domain.Extensions;

public static class DateTimeExtensions
{
    public static double GetDifferenceInSeconds(this DateTime minuend, DateTime subtrahend)
    {
        return (minuend - subtrahend).TotalSeconds;
    }

    public static ICollection<DateTime> GetDatesUntil(this DateTime dateFrom, DateTime dateTo)
    {
        return Enumerable.Range(0, (dateTo - dateFrom).Days + 1)
            .Select(num => dateFrom.AddDays(num)).ToArray();
    }
}