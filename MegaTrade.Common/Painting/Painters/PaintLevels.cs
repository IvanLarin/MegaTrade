using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Painting.Painters;

internal class PaintLevels : PaintBase, IPaintLevels
{
    private const int Opacity = 90;

    public void Level(double value, string name, Color color) => DrawLevel(value, name, color, out var usedColor);

    public void Level(double value, string name, Color color, out Color usedColor) =>
        DrawLevel(value, name, color, out usedColor);

    public void Level(double value, string name, AnimalColor animalColor) => DrawLevel(value, name,
        ChooseAnimalPalette(animalColor).PopColor(), out var usedColor);

    public void Level(double value, string name, AnimalColor animalColor, out Color usedColor) =>
        DrawLevel(value, name, ChooseAnimalPalette(animalColor).PopColor(), out usedColor);

    private void DrawLevel(double value, string name, Color colorToDraw, out Color usedColor)
    {
        List<double> values = Enumerable.Range(0, Context.BarsCount).Select(_ => value).ToList();
        var chart = Graph.AddList(
            name,
            values,
            ListStyles.LINE,
            usedColor = colorToDraw,
            LineStyles.DASH,
            PaneSides.RIGHT);
        chart.Autoscaling = true;
        chart.Opacity = Opacity;
        Graph.UpdatePrecision(PaneSides.RIGHT, 2);
    }
    public required IContext Context { private get; init; }
}