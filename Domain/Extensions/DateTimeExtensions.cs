namespace Domain.Extensions;

public static class DateTimeExtensions
{
    public static double GetDifferenceInSeconds(this DateTime minuend, DateTime subtrahend)
    {
        return (minuend - subtrahend).TotalSeconds;
    }
}