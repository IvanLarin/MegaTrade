using MegaTrade.Common.Caching;
using MegaTrade.Common.Extensions;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Basic.Indicating;

public class Indicators : IIndicators
{
    public IList<double> MACD(IList<double> source, int smallPeriod, int bigPeriod) => Cache
        .Entry<IList<double>>("MACD", CacheKind.Memory, [source.CacheKey(), smallPeriod, bigPeriod])
        .Calculate(() => new MACDEx
        {
            Period1 = smallPeriod,
            Period2 = bigPeriod
        }.Execute(source));

    public IList<double> RSI(IList<double> source, int period) => Cache
        .Entry<IList<double>>("RSI", CacheKind.Memory, [source.CacheKey(), period])
        .Calculate(() => new RSI
        {
            Period = period
        }.Execute(source));

    public IList<double> ATR(ISecurity source, int period) => Cache
        .Entry<IList<double>>("ATR", CacheKind.Memory, [source.CacheKey(), period])
        .Calculate(() => new AverageTrueRangeNew
        {
            Period = period
        }.Execute(source));
}