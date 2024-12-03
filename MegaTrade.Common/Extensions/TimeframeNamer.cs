using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Common.Extensions;

internal class TimeframeNamer(ISecurity security)
{
    public string GetName() => security.Symbol + " " + (
        security.IntervalBase == DataIntervals.MINUTE
            ? GetTimeframeName()
            : FallbackName);

    private string GetTimeframeName() =>
        security.IntervalBase == DataIntervals.MINUTE
            ? MonthName ??
              WeekName ??
              DayName ??
              HourName ??
              MinuteName ??
              FallbackName
            : FallbackName;

    private string? MonthName => GetName("Month", 30 * 24 * 60);

    private string? WeekName => GetName("W", 7 * 24 * 60);

    private string? DayName => GetName("D", 24 * 60);

    private string? HourName => GetName("H", 60);

    private string? MinuteName => GetName("M", 1);

    private string? GetName(string postfix, int minutesPerInterval) =>
        security.Interval.IsDivisibleBy(minutesPerInterval)
            ? $"{security.Interval / minutesPerInterval}{postfix}"
            : null;

    private string FallbackName =>
        $"{security.Interval}{Enum.GetName(security.IntervalBase)}";
}