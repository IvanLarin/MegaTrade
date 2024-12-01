using MegaTrade.Common.Caching;
using MegaTrade.Common.Extensions;
using TSLab.Script.Handlers;

namespace MegaTrade.Common;

public class TheIndicatorFactory(IContext context) : IndicatorFactory
{
    public IList<double> MakeMacd(IList<double> source, int smallPeriod, int bigPeriod) => Cache
        .Entry<IList<double>>("MACD", CacheKind.Memory, context, [source.GenerateCacheKey(), smallPeriod, bigPeriod])
        .Calculate(() => new MACDEx
        {
            Period1 = smallPeriod,
            Period2 = bigPeriod
        }.Execute(source));

    public IList<double> MakeRsi(IList<double> source, int period) => Cache
        .Entry<IList<double>>("RSI", CacheKind.Memory, context, [source.GenerateCacheKey(), period])
        .Calculate(() => new RSI
        {
            Period = period
        }.Execute(source));
}