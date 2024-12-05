using MegaTrade.Common.Extensions;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Painting.Painters;

internal class PaintBounds : IPaintBounds
{
    private const int Opacity = 99999;

    private const int DefaultMajorityPercent = 95;

    public void Bound(params double[] bounds) => bounds.ToList().ForEach(DrawBound);

    public void BoundOfMin(params IList<double>[] bounds) => DrawBound(bounds.SelectMany(x => x).Min());

    public void BoundOfMax(params IList<double>[] bounds) => DrawBound(bounds.SelectMany(x => x).Max());

    public void BoundOfMajorMax(params IList<double>[] bounds) =>
        DrawBound(bounds.SelectMany(x => x).Where(x => x >= 0).Percentile(DefaultMajorityPercent));

    public void BoundOfMajorMin(params IList<double>[] bounds) =>
        DrawBound(bounds.SelectMany(x => x).Where(x => x <= 0).Percentile(100 - DefaultMajorityPercent));

    public void BoundOfMajorMax(double majorityPercent, IList<double>[] bounds) =>
        DrawBound(bounds.SelectMany(x => x).Where(x => x >= 0).Percentile(majorityPercent));

    public void BoundOfMajorMin(double majorityPercent, IList<double>[] bounds) =>
        DrawBound(bounds.SelectMany(x => x).Where(x => x <= 0).Percentile(100 - majorityPercent));

    private void DrawBound(double bound)
    {
        List<double> values = Enumerable.Range(0, Context.BarsCount).Select(_ => bound).ToList();
        var chart = Graph.AddList(
            "Граница",
            values,
            ListStyles.LINE,
            ScriptColors.White,
            LineStyles.SOLID,
            PaneSides.RIGHT);
        chart.Autoscaling = true;
        chart.Opacity = Opacity;
    }

    public required IGraphPane Graph { protected get; init; }

    public required IContext Context { private get; init; }
}