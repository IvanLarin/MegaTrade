using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Painting;

internal class PaintLevels : IPaintLevels
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

    private IPalette ChooseAnimalPalette(AnimalColor animalColor) =>
        animalColor == AnimalColor.Bull ? BullPalette :
        animalColor == AnimalColor.Bear ? BearPalette : NeutralPalette;

    public required IPalette BullPalette { private get; init; }

    public required IPalette BearPalette { private get; init; }

    public required IPalette NeutralPalette { private get; init; }

    public required IGraphPane Graph { private get; init; }

    public required IContext Context { private get; init; }
}