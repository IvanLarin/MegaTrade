using MegaTrade.Indicators.All;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Indicators;

public static class Indicators
{
    public static IList<double> MACD(this IList<double> source, int smallPeriod, int bigPeriod) =>
        new MACDEx
        {
            Period1 = smallPeriod,
            Period2 = bigPeriod
        }.Execute(source);

    public static IList<double> RSI(this IList<double> source, int period) =>
        new RSI
        {
            Period = period
        }.Execute(source);

    public static IList<double> ATR(this ISecurity source, int period) =>
        new AverageTrueRangeNew
        {
            Period = period
        }.Execute(source);

    public static IList<double> NadarayaWatsonUpper(this IList<double> source, double bandwidth, double multiplier,
        int range) =>
        new NadarayaWatsonBand
        {
            Band = Band.Upper,
            Multiplier = multiplier,
            Source = NadarayaWatson(source, bandwidth, range)
        }.Result;

    public static IList<double> NadarayaWatsonLower(this IList<double> source, double bandwidth, double multiplier,
        int range) =>
        new NadarayaWatsonBand
        {
            Band = Band.Lower,
            Multiplier = multiplier,
            Source = NadarayaWatson(source, bandwidth, range)
        }.Result;

    public static (double[] nwe, double[] sae) NadarayaWatson(IList<double> source, double bandwidth, int range) =>
        new NadarayaWatson
        {
            Source = source,
            Bandwidth = bandwidth,
            Range = range
        }.Result;
}