using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Basic.TradeExtensions;

public static class Extensions
{
    public static int To(this int barIndex, ISecurity toTimeframe) =>
        new BarIndexToTimeframe
        {
            BarIndex = barIndex,
            ToTimeframe = toTimeframe
        }.Result;

    public static IList<T> DecompressFrom<T>(this IList<T> source, ISecurity fromTimeframe) where T : struct =>
        fromTimeframe.Decompress(source, DecompressMethodWithDef.Default);
}