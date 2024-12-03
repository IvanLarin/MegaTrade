using TSLab.Script;

namespace MegaTrade.Common.Painting;

public class PaintFunctions(IGraphPane graph, IPalette palette) : IPaintFunctions
{
    public void Function(IList<double> values, string name, Color? color = null) =>
        graph.AddList(
            name,
            values,
            ListStyles.LINE,
            color ?? palette.PopColor(),
            LineStyles.SOLID,
            PaneSides.RIGHT);
}