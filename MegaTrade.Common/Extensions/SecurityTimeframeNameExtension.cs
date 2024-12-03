using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Common.Extensions;

internal static class SecurityTimeframeNameExtension
{
    public static string GetTimeframeName(this ISecurity security) =>
        security.Symbol + " " + (
            security.IntervalBase == DataIntervals.MINUTE
                ? GetTimeframeName(security.IntervalBase, security.Interval)
                : Enum.GetName(security.IntervalBase));

    private static string GetTimeframeName(DataIntervals intervalBase, int interval) =>
        intervalBase == DataIntervals.MINUTE
            ? GetMonthName(interval) ??
              GetWeekName(interval) ??
              GetDayName(interval) ??
              Get4HourName(interval) ??
              Get3HourName(interval) ??
              Get2HourName(interval) ??
              Get1HourName(interval) ??
              Get45MinuteName(interval) ??
              Get30MinuteName(interval) ??
              Get15MinuteName(interval) ??
              Get10MinuteName(interval) ??
              Get5MinuteName(interval) ??
              Get3MinuteName(interval) ??
              Get2MinuteName(interval) ??
              Get1MinuteName(interval) ??
              GetFallbackName(intervalBase, interval)
            : GetFallbackName(intervalBase, interval);

    private static string? GetMonthName(int interval) => GetName(interval, "1Month", 30 * 24 * 60);

    private static string? GetWeekName(int interval) => GetName(interval, "1W", 7 * 24 * 60);

    private static string? GetDayName(int interval) => GetName(interval, "1D", 24 * 60);

    private static string? Get4HourName(int interval) => GetName(interval, "4H", 4 * 60);

    private static string? Get3HourName(int interval) => GetName(interval, "3H", 3 * 60);

    private static string? Get2HourName(int interval) => GetName(interval, "2H", 2 * 60);

    private static string? Get1HourName(int interval) => GetName(interval, "1H", 1 * 60);

    private static string? Get45MinuteName(int interval) => GetName(interval, "45M", 45);

    private static string? Get30MinuteName(int interval) => GetName(interval, "30M", 30);

    private static string? Get15MinuteName(int interval) => GetName(interval, "15M", 15);

    private static string? Get10MinuteName(int interval) => GetName(interval, "10M", 10);

    private static string? Get5MinuteName(int interval) => GetName(interval, "5M", 5);

    private static string? Get3MinuteName(int interval) => GetName(interval, "3M", 3);

    private static string? Get2MinuteName(int interval) => GetName(interval, "2M", 2);

    private static string? Get1MinuteName(int interval) => GetName(interval, "1M", 1);

    private static string? GetName(int interval, string postfix, int minutesPerInterval) =>
        interval.IsDivisibleBy(minutesPerInterval) ? postfix : null;

    private static string GetFallbackName(DataIntervals intervalBase, int interval) =>
        $"{interval} {Enum.GetName(intervalBase)}";
}