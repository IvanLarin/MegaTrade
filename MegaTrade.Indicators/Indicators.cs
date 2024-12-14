using MegaTrade.Common.Caching;
using MegaTrade.Common.Extensions;
using MegaTrade.Indicators.All;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Indicators;

public static class Indicators
{
    public static IList<double> MACD(this IList<double> source, int smallPeriod, int bigPeriod) => Cache
        .Entry<IList<double>>("MACD", CacheKind.Memory, [source.CacheKey(), smallPeriod, bigPeriod])
        .Calculate(() => new MACDEx
        {
            Period1 = smallPeriod,
            Period2 = bigPeriod
        }.Execute(source));

    public static IList<double> RSI(this IList<double> source, int period) => Cache
        .Entry<IList<double>>("RSI", CacheKind.Memory, [source.CacheKey(), period])
        .Calculate(() => new RSI
        {
            Period = period
        }.Execute(source));

    public static IList<double> ATR(this ISecurity source, int period) => Cache
        .Entry<IList<double>>("ATR", CacheKind.Memory, [source.CacheKey(), period])
        .Calculate(() => new AverageTrueRangeNew
        {
            Period = period
        }.Execute(source));

    public static IList<double> NadarayaWatsonUp(this IList<double> source, double bandwidth, double multiplier,
        int range) => NadarayaWatson(source, Direction.Up, bandwidth, multiplier, range);

    public static IList<double> NadarayaWatsonDown(this IList<double> source, double bandwidth, double multiplier,
        int range) => NadarayaWatson(source, Direction.Down, bandwidth, multiplier, range);

    private static IList<double> NadarayaWatson(IList<double> source, Direction direction, double bandwidth, double multiplier,
        int range) => Cache
        .Entry<IList<double>>("Nadaraya Watson", CacheKind.DiskAndMemory, [source.CacheKey(), direction, direction, bandwidth, multiplier])
        .Calculate(() => new NadarayaWatson
        {
            Source = source,
            Direction = direction,
            Bandwidth = bandwidth,
            Multiplier = multiplier,
            Range = range,
        }.Result);
}