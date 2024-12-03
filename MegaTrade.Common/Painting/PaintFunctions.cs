using TSLab.Script;

namespace MegaTrade.Common.Painting;

public class PaintFunctions : IPaintFunctions
{
    public void Function(IList<double> values, string name, Color? color = null) =>
        Graph.AddList(
            name,
            values,
            ListStyles.LINE,
            color ?? Palette.PopColor(),
            LineStyles.SOLID,
            PaneSides.RIGHT);

    public required IPalette Palette { private get; init; }

    public required IGraphPane Graph { private get; init; }
}