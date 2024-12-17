using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Common.Extensions
{
    public static class SecurityExtensions
    {
        public static string GetTimeframeName(this ISecurity timeframe) => new TimeframeNamer(timeframe).GetName();

        public static ISecurity DailyCompressTo(this ISecurity timeframe, int toInterval) =>
            timeframe.CompressTo(new Interval(toInterval, timeframe.IntervalBase), 0, 1440, 0);
    }
}
