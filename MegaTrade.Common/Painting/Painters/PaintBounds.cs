using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Painting.Painters;

internal class PaintBounds : IPaintBounds
{
    private const int Opacity = 99999;

    public void Bound(double bound)
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
        Graph.UpdatePrecision(PaneSides.RIGHT, 2);
    }

    public required IGraphPane Graph { protected get; init; }

    public required IContext Context { private get; init; }
}