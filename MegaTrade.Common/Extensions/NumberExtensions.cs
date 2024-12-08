using TSLab.Script;

namespace MegaTrade.Common.Extensions;

public static class NumberExtensions
{
    public static bool IsEqualTo(this double number, double another) => Math.Abs(number - another) < double.Epsilon;

    public static bool IsMoreThan(this double number, double another) => number - another > double.Epsilon;

    public static bool IsLessOrEqualTo(this double number, double another) =>
        another.IsMoreThan(number) || another.IsEqualTo(number);

    public static bool IsDivisibleBy(this int number, int by) => number % by == 0;

    public static int To(this int fromBarIndex, ISecurity timeframe)
    {
        var bars = timeframe.Bars;

        if (bars[0].OriginalFirstIndex == -1)
            return fromBarIndex;

        var left = 0;
        var right = bars.Count - 1;

        while (true)
        {
            var center = (right + left) / 2;
            var bar = bars[center];

            if (bar.OriginalFirstIndex <= fromBarIndex && fromBarIndex <= bar.OriginalLastIndex)
                if (fromBarIndex == bar.OriginalLastIndex)
                    return center;
                else
                    return Math.Max(0, center - 1);

            if (fromBarIndex < bar.OriginalFirstIndex)
                right = center - 1;
            else
                left = center + 1;

            if (left == right)
                if (fromBarIndex == bars[left].OriginalLastIndex)
                    return left;
                else
                    return Math.Max(0, left - 1);
        }
    }
}