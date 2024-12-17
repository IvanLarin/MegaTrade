using MegaTrade.Common.Calculating;
using TSLab.Script;

namespace MegaTrade.Basic.TradeExtensions;

internal class BarIndexToTimeframe : Calculator<int>
{
    protected override int Calculate()
    {
        var bars = ToTimeframe.Bars;

        if (bars[0].OriginalFirstIndex == -1)
            return BarIndex;

        var left = 0;
        var right = bars.Count - 1;

        while (true)
        {
            var center = (right + left) / 2;
            var bar = bars[center];

            if (bar.OriginalFirstIndex <= BarIndex && BarIndex <= bar.OriginalLastIndex)
                if (BarIndex == bar.OriginalLastIndex)
                    return center;
                else
                    return Math.Max(0, center - 1);

            if (BarIndex < bar.OriginalFirstIndex)
                right = center - 1;
            else
                left = center + 1;

            if (left == right)
                if (BarIndex == bars[left].OriginalLastIndex)
                    return left;
                else
                    return Math.Max(0, left - 1);
        }
    }

    public required int BarIndex { private get; init; }

    public required ISecurity ToTimeframe { private get; init; }
}