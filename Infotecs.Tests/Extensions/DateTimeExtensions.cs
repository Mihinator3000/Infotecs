namespace Infotecs.Tests.Extensions;

public static class DateTimeExtensions
{
    public const int TicksInSecond = 10_000_000;

    public static long GetTotalSeconds(this DateTime dateTime)
        => dateTime.Ticks / TicksInSecond;
}