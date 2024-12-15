using MegaTrade.Common.Caching;
using MegaTrade.Common.Extensions;
using MegaTrade.Indicators.All;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Indicators;

public static class Indicators
{
    public static IList<double> MACD(this IList<double> source, int smallPeriod, int bigPeriod) =>
        Cache.Entry<IList<double>>(nameof(MACD), CacheKind.Memory, [source.CacheKey(), smallPeriod, bigPeriod])
            .Calculate(() => new MACDEx
            {
                Period1 = smallPeriod,
                Period2 = bigPeriod
            }.Execute(source));

    public static IList<double> RSI(this IList<double> source, int period) =>
        Cache.Entry<IList<double>>(nameof(RSI), CacheKind.Memory, [source.CacheKey(), period])
            .Calculate(() => new RSI
            {
                Period = period
            }.Execute(source));

    public static IList<double> ATR(this ISecurity source, int period) =>
        Cache.Entry<IList<double>>(nameof(ATR), CacheKind.Memory, [source.CacheKey(), period])
            .Calculate(() => new AverageTrueRangeNew
            {
                Period = period
            }.Execute(source));

    public static IList<double> NadarayaWatsonUpper(this IList<double> source, double bandwidth, double multiplier,
        int range) =>
        Cache.Entry<IList<double>>(nameof(NadarayaWatsonUpper), CacheKind.Memory,
                [source.CacheKey(), bandwidth, multiplier, range, Band.Upper])
            .Calculate(() => new NadarayaWatsonBand
            {
                Band = Band.Upper,
                Multiplier = multiplier,
                Input = NadarayaWatson(source, bandwidth, range)
            }.Result);

    public static IList<double> NadarayaWatsonLower(this IList<double> source, double bandwidth, double multiplier,
        int range) =>
        Cache.Entry<IList<double>>(nameof(NadarayaWatsonLower), CacheKind.Memory,
                [source.CacheKey(), bandwidth, multiplier, range, Band.Lower])
            .Calculate(() => new NadarayaWatsonBand
            {
                Band = Band.Lower,
                Multiplier = multiplier,
                Input = NadarayaWatson(source, bandwidth, range)
            }.Result);

    public static (double[] nwe, double[] sae) NadarayaWatson(IList<double> source, double bandwidth, int range) =>
        Cache.Entry<(double[] nwe, double[] sae)>(nameof(NadarayaWatson), CacheKind.Memory,
                [source.CacheKey(), bandwidth, range])
            .Calculate(() => new NadarayaWatson
            {
                Source = source,
                Bandwidth = bandwidth,
                Range = range
            }.Result);
}