using TSLab.Script;

namespace MegaTrade.Common.Painting;

public class PaintHistogram : IPaintHistogram
{
    private const int SignalOpacity = 150;

    public void Histogram(IList<bool> values, string name, AnimalColor animalColor = AnimalColor.Neutral) =>
        DrawHistogram(values, name, ChooseAnimalPalette(animalColor).PopColor());

    public void Histogram(IList<bool> values, string name, Color color) =>
        DrawHistogram(values, name, color);

    public void Signal(IList<bool> values, string name, AnimalColor animalColor = AnimalColor.Neutral) =>
        DrawHistogram(values, name, ChooseAnimalPalette(animalColor).PopColor(), SignalOpacity);

    public void Signal(IList<bool> values, string name, Color color) =>
        DrawHistogram(values, name, color, SignalOpacity);

    private void DrawHistogram(IList<bool> values, string name, Color color, int opacity = 0)
    {
        var chart = Graph.AddList(
            name,
            values,
            ListStyles.HISTOHRAM,
            color,
            LineStyles.SOLID,
            PaneSides.LEFT);

        chart.Opacity = opacity;
        chart.Autoscaling = true;
    }

    private IPalette ChooseAnimalPalette(AnimalColor animalColor) =>
        animalColor == AnimalColor.Bull ? BullPalette :
        animalColor == AnimalColor.Bear ? BearPalette : NeutralPalette;

    public required IPalette BullPalette { private get; init; }

    public required IPalette BearPalette { private get; init; }

    public required IPalette NeutralPalette { private get; init; }

    public required IGraphPane Graph { private get; init; }
}