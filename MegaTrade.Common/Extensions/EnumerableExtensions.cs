using MegaTrade.Common.Calculating;

namespace MegaTrade.Common.Extensions;

public static class EnumerableExtensions
{
    public static double Percentile(this IEnumerable<double> values, double percentile) =>
        new CalculatePercentile
        {
            Values = values.ToArray(),
            Percentile = percentile
        }.Result;
}