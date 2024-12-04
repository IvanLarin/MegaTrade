using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal class PaintFunctions : IPaintFunctions
{
    public void Function(IList<double> values, string name, Color? color = null) =>
        DrawFunction(values, name, out var usedColor, color);

    public void Function(IList<double> values, string name, out Color usedColor) =>
        DrawFunction(values, name, out usedColor);

    private void DrawFunction(IList<double> values, string name, out Color usedColor, Color? colorToDraw = null) =>
        Graph.AddList(
            name,
            values,
            ListStyles.LINE,
            usedColor = colorToDraw ?? Palette.PopColor(),
            LineStyles.SOLID,
            PaneSides.RIGHT);

    public required IPalette Palette { private get; init; }

    public required IGraphPane Graph { private get; init; }
}